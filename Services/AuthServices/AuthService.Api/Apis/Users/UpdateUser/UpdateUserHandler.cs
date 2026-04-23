namespace AuthService.Api.Apis.Users.UpdateUser;

public record UpdateUserCommand(Guid Id, string Username, Status Status, IEnumerable<Guid> Roles, IEnumerable<Guid> Claims, IEnumerable<ScopeDto> Scopes) : IRequest<UpdateUserResult>;
public record UpdateUserResult(Guid Id);

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(256);
    }
}

internal class UpdateUserHandler(AuthServiceDbContext context, ITenantGuard tenantGuard) : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
            .Include(u => u.UserClaims)
            .Include(u => u.UserScopes).ThenInclude(us => us.Scope)
            .FirstOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

        if (user == null) throw new NotFoundException("User not found.");

        tenantGuard.EnsureCanAccess(user.TenantId);

        user.Username = command.Username;
        user.Status = command.Status;

        // update roles
        var roles = context.Roles.Where(r => command.Roles.Contains(r.Id)).ToList();
        if (roles.Count != command.Roles.Count()) throw new NotFoundException("One or more roles not found.");
        user.UserRoles.Clear();
        user.UserRoles = roles.Select(r => new UserRole { RoleId = r.Id }).ToList();

        // update claims
        var claims = context.Claims.Where(c => command.Claims.Contains(c.Id)).ToList();
        if (claims.Count != command.Claims.Count()) throw new NotFoundException("One or more claims not found.");
        user.UserClaims.Clear();
        user.UserClaims = claims.Select(c => new UserClaim { ClaimId = c.Id }).ToList();

        // update scopes
        user.UserScopes.Clear();
        foreach(var scopeDto in command.Scopes)
        {
            var scope = context.Scopes.FirstOrDefault(s => s.Type == scopeDto.Type && s.Value == scopeDto.Value);
            if(scope == null)
            {
                user.UserScopes.Add(new UserScope { Scope = new Scope { Type = scopeDto.Type, Value = scopeDto.Value } });
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
