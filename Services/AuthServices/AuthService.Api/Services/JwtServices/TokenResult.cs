namespace AuthService.Api.Services.JwtServices;

public class TokenResult
{
    public string AccessToken { get; init; } = default!;
    public string RefreshToken { get; init; } = default!;

    public DateTime AccessTokenExpiresAtUtc { get; init; }
    public DateTime RefreshTokenExpiresAtUtc { get; init; }

    public string TokenType { get; init; } = "Bearer";
}
