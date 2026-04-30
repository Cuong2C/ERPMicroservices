using AuthService.Api.Services.JwtServices.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.Api.Apis.Tokens.RefeshToken;

public record RefeshTokenCommand(string RefreshToken) : IRequest<RefeshTokenResult>;

public record RefeshTokenResult(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc, DateTime RefreshTokenExpiresAtUtc);

internal class RefeshTokenHandler(AuthServiceDbContext context, IJwtService jwtService) : IRequestHandler<RefeshTokenCommand, RefeshTokenResult>
{
    public async Task<RefeshTokenResult> Handle(RefeshTokenCommand request, CancellationToken cancellationToken)
    {
        var claimsPrincipal = jwtService.ValidateRefreshToken(request.RefreshToken);

        var userId = claimsPrincipal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (userId is null)
            throw new UnauthorizedException("Invalid refresh token.");

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id.ToString().Equals(userId), cancellationToken);

        if (user is null)
            throw new BadRequestException("Invalid refresh token");

        var newAccessToken = jwtService.GenerateToken(user);

        return new RefeshTokenResult(
            AccessToken: newAccessToken.AccessToken,
            RefreshToken: newAccessToken.RefreshToken,
            AccessTokenExpiresAtUtc: newAccessToken.AccessTokenExpiresAtUtc,
            RefreshTokenExpiresAtUtc: newAccessToken.RefreshTokenExpiresAtUtc
        );

    }
}
