namespace AuthService.Api.Apis.Users.UpdateUser;

public record UpdateUserCommand(Guid Id, Status Status, string Fullname, string? Email, string? Address, string? City, string? Region, int? PostalCode, string? Country, string? PhoneNumber, IEnumerable<Guid> Roles, IEnumerable<Guid> Claims, IEnumerable<ScopeDto> Scopes) : IRequest<UpdateUserResult>;
public record UpdateUserResult(Guid Id);

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Fullname).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.PostalCode).GreaterThan(0).When(x => x.PostalCode.HasValue);
    }
}

internal class UpdateUserHandler(AuthServiceDbContext context, IUserGuard userGuard) : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
            .Include(u => u.UserPermissions)
            .Include(u => u.UserScopes).ThenInclude(us => us.Scope)
            .FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

        if (user == null) throw new NotFoundException("User not found.");

        userGuard.EnsureCanAccess(user.Id);

        user.Fullname = command.Fullname;
        user.Status = command.Status;
        user.Email = command.Email;
        user.Address = command.Address;
        user.City = command.City;
        user.Region = command.Region;
        user.PostalCode = command.PostalCode;
        user.Country = command.Country;
        user.PhoneNumber = command.PhoneNumber;

        var adminOrRootAdmin = context.UserRoles
           .Where(ur => ur.Role.Name == StaticDetail.ROLE_SHOP_OWNER || ur.Role.Name == StaticDetail.ROLE_ROOT_ADMIN)
           .Select(ur => ur.RoleId)
           .ToHashSet();

        if (command.Roles.Any(r => adminOrRootAdmin.Contains(r)))
        {
            throw new BadRequestException("Cannot assign admin roles to this user.");
        }

        // update roles
        var roles = context.Roles.Where(r => command.Roles.Contains(r.Id)).ToList();
        if (roles.Count != command.Roles.Count()) throw new NotFoundException("One or more roles not found.");
        user.UserRoles.Clear();
        user.UserRoles = roles.Select(r => new UserRole { RoleId = r.Id }).ToList();

        // update claims
        var claims = context.Permissions.Where(c => command.Claims.Contains(c.Id)).ToList();
        if (claims.Count != command.Claims.Count()) throw new NotFoundException("One or more claims not found.");
        user.UserPermissions.Clear();
        user.UserPermissions = claims.Select(c => new UserPermission { PermissionId = c.Id }).ToList();

        // update scopes
        user.UserScopes.Clear();
        foreach(var scopeDto in command.Scopes)
        {
            var scope = context.Scopes.FirstOrDefault(s => s.ResourceId == scopeDto.ResourceId && s.Value == scopeDto.Value);
            if(scope == null)
            {
                user.UserScopes.Add(new UserScope { Scope = new Scope { ResourceId = scopeDto.ResourceId, Value = scopeDto.Value } });
            }
            else
            {
                user.UserScopes.Add(new UserScope { ScopeId = scope.Id });
            }
        }

        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateUserResult(user.Id);
    }
}
