using AuthService.Api.Data;
using AuthService.Api.Identity;

namespace AuthService.Api.Apis.Users.Register;

public record RegisterUserCommand(string Username, string Password, Guid TenantId) : IRequest<RegisterUserResult>;
public record RegisterUserResult(Guid Id);

public class RegisterUserHandler(AuthServiceDbContext context) : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = CustomHasher.HashByArgon2(request.Password),
            TenantId = request.TenantId
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync(cancellationToken);

        return new RegisterUserResult(user.Id);
    }
}
