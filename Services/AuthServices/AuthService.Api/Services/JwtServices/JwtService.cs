using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthService.Api.Services.JwtServices;

public class JwtService(JwtOptions jwtOption) : IJwtService
{
    public TokenResult GenerateToken(User user)
    {
        var nowUtc = DateTime.UtcNow;

        var accessJti = Guid.NewGuid().ToString();
        var refreshJti = Guid.NewGuid().ToString();
        var accessExpires = nowUtc.AddMinutes(jwtOption.AccessTokenMinutes);
        var refreshExpires = nowUtc.AddDays(jwtOption.RefreshTokenDays);

        var accessToken = BuildAccessToken(user, accessJti, accessExpires);
        var refreshToken = BuildRefeshToken(user, refreshJti, refreshExpires);

        return new TokenResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAtUtc = accessExpires,
            RefreshTokenExpiresAtUtc = refreshExpires,
        };
    }

    private string BuildRefeshToken(User user, string refreshJti, DateTime refreshExpires)
    {
        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.UniqueName, user.Username),
            new (JwtRegisteredClaimNames.Jti, refreshJti),
            new ("token_type", "refresh"),
            new ("tennant_id", user.TenantId.ToString())
        };

        return BuildJwt(claims, refreshExpires);
    }

    private string BuildAccessToken(User user, string accessJti, DateTime accessExpires)
    {
        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.UniqueName, user.Username),
            new (JwtRegisteredClaimNames.Jti, accessJti),
            new ("token_type", "access"),
            new ("tennant_id", user.TenantId.ToString())
        };

        var permissions = new HashSet<string>();

        foreach (var role in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));

            foreach (var rolePermission in role.Role.RolePermissions)
            {
                permissions.Add(rolePermission.Permission.Code);
            }
        }

        foreach (var permission in user.UserPermissions)
        {
            permissions.Add(permission.Permission.Code);
        }

        foreach(var permission in permissions)
        {
            claims.Add(new Claim("permission", permission));
        }

        foreach (var userScope in user.UserScopes)
        {
            claims.Add(new Claim("scope", $"{userScope.Scope.Type}:{userScope.Scope.Value}"));
        }

        return BuildJwt(claims, accessExpires);
    }

    private string BuildJwt(List<Claim> claims, DateTime expires)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(jwtOption.PrivateKey), out _);

        var signingCredentials = new SigningCredentials(
            new RsaSecurityKey(rsa)
            {
                KeyId = jwtOption.KeyId
            },
            SecurityAlgorithms.RsaSha256
        );

        var token = new JwtSecurityToken(
            issuer: jwtOption.Issuer,
            audience: jwtOption.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: signingCredentials
        );

        token.Header["kid"] = jwtOption.KeyId;

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateAccessToken(string token)
    {
        var principal = ValidateToken(token, true);

        if (principal == null)
            return null;

        var tokenType = principal.FindFirst("token_type")?.Value;

        return tokenType == "access"
            ? principal
            : null;
    }

    public ClaimsPrincipal? ValidateRefreshToken(string token)
    {
        var principal = ValidateToken(token, true);

        if (principal == null)
            return null;

        var tokenType = principal.FindFirst("token_type")?.Value;

        return tokenType == "access"
            ? principal
            : null;
    }

    private ClaimsPrincipal? ValidateToken(string token, bool validateLifetime)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(jwtOption.PublicKey), out _);

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOption.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOption.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.Zero
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, parameters, out _);

        //var jti = principal.FindFirst("jti")?.Value;
        // Check if the jti is revoked or not 
        return principal;
    }
}
