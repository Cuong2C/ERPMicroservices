namespace AuthService.Api.Services.JwtServices;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string PrivateKey { get; set; } = default!;
    public string PublicKey { get; set; } = default!;
    public string KeyId { get; set; } = "key-1";
    public int AccessTokenMinutes { get; set; } = 30;
    public int RefreshTokenDays { get; set; } = 7;
}
