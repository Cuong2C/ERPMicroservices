using AuthService.Api.Identity;
using AuthService.Api.Services.JwtServices;

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
            throw new UnauthorizedException("Invalid username or password");

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
