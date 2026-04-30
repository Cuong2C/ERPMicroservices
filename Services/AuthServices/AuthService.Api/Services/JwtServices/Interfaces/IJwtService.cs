using System.Security.Claims;

namespace AuthService.Api.Services.JwtServices.Interfaces;

public interface IJwtService
{
    TokenResult GenerateToken(User user);

    ClaimsPrincipal? ValidateAccessToken(string token);

    ClaimsPrincipal? ValidateRefreshToken(string token);

}
