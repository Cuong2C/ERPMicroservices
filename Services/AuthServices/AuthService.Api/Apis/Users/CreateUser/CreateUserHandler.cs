using AuthService.Api.Identity;
using FluentValidation;

namespace AuthService.Api.Apis.Users.CreateUser;

public record CreateUserCommand(
    string Username,
    string Password,
    IEnumerable<Guid> Roles,
    IEnumerable<Guid> Claims,
    IEnumerable<ScopeDto> Scopes
) : IRequest<CreateUserResult>;

public record CreateUserResult(Guid Id);

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

public class CreateUserHandler(AuthServiceDbContext context) : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Username = command.Username,
            PasswordHash = CustomHasher.HashByArgon2(command.Password)
        };

        var roles = context.Roles.Where(r => command.Roles.Contains(r.Id));
        if(roles.Count() != command.Roles.Count())
        {
            throw new NotFoundException("One or more roles not found.");
        }

        user.UserRoles = roles.Select(r => new UserRole { RoleId = r.Id }).ToList();

        var claims = context.Claims.Where(c => command.Claims.Contains(c.Id));
        if(claims.Count() != command.Claims.Count())
        {
            throw new NotFoundException("One or more claims not found.");
        }

        user.UserClaims = claims.Select(c => new UserClaim { ClaimId = c.Id }).ToList();

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

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateUserResult(user.Id);
    }
}
