using AuthService.Api.Services.JwtServices.Interfaces;

namespace AuthService.Api.Apis.Users.Login;
public record LoginCommand(string Username, string Password) : IRequest<LoginResult>;

public record LoginResult(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc, DateTime RefreshTokenExpiresAtUtc);

internal class LoginHandler(AuthServiceDbContext context, IJwtService jwtService) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = context.Users.FirstOrDefault(u => u.Username == request.Username);

        if (user is null) 
            throw new UnauthorizedException("Invalid username or password");

        if (!CustomHasher.VerifyByArgon2(user.PasswordHash, request.Password))
        {
            
            if (user.AccessFailedCount >= 4)
            {
                user.AccessFailedCount = 0;
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                await context.SaveChangesAsync(cancellationToken);
                throw new UnauthorizedException("Account locked due to multiple failed login attempts");
            }

            user.AccessFailedCount++;
            await context.SaveChangesAsync(cancellationToken);
            throw new UnauthorizedException("Invalid username or password");
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            throw new ForbiddenException("Account locked in 15 minutes due to multiple failed login attempts");

        if(user.IsLocked)
            throw new ForbiddenException("Account is locked");

        // Generating tokens and returning the result
        var accessToken = jwtService.GenerateToken(user);

        return new LoginResult(
            AccessToken: accessToken.AccessToken,
            RefreshToken: accessToken.RefreshToken,
            AccessTokenExpiresAtUtc: accessToken.AccessTokenExpiresAtUtc,
            RefreshTokenExpiresAtUtc: accessToken.RefreshTokenExpiresAtUtc
        );
    }
}
