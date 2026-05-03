using AuthService.Api.Services.JwtServices.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthService.Api.Services.JwtServices;

public class JwtService(AuthServiceDbContext context, IOptions<JwtOptions> jwtOption, IOptions<RsaKeyProvider> rsaKeyProvider) : IJwtService
{
    public TokenResult GenerateToken(User user)
    {
        var nowUtc = DateTime.UtcNow;

        var accessJti = Guid.NewGuid().ToString();
        var refreshJti = Guid.NewGuid().ToString();
        var accessExpires = nowUtc.AddMinutes(jwtOption.Value.AccessTokenMinutes);
        var refreshExpires = nowUtc.AddDays(jwtOption.Value.RefreshTokenDays);

        var accessToken = BuildAccessToken(user, accessJti, accessExpires);
        var refreshToken = BuildRefeshToken(user, refreshJti, refreshExpires);

        return new TokenResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAtUtc = accessExpires,
            RefreshTokenExpiresAtUtc = refreshExpires
        };
    }

    private string BuildRefeshToken(User user, string refreshJti, DateTime refreshExpires)
    {
        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.UniqueName, user.Username),
            new (JwtRegisteredClaimNames.Jti, refreshJti),
            new (StaticDetail.CLAIM_TYPE_TOKEN_TYPE, StaticDetail.TOKEN_TYPE_REFRESH),
            new (StaticDetail.CLAIM_TYPE_TENANT_ID, user.TenantId ?? "")
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
            new (StaticDetail.CLAIM_TYPE_TOKEN_TYPE, StaticDetail.TOKEN_TYPE_ACCESS),
            new (StaticDetail.CLAIM_TYPE_TENANT_ID, user.TenantId ?? "")
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
            claims.Add(new Claim(StaticDetail.CLAIM_TYPE_PERMISSIONS, permission));
        }

        foreach (var userScope in user.UserScopes)
        {
            claims.Add(new Claim(StaticDetail.CLAIM_TYPE_SCOPES, $"{userScope.Scope.ResourceId}:{userScope.Scope.Value}"));
        }

        return BuildJwt(claims, accessExpires);
    }

    private string BuildJwt(List<Claim> claims, DateTime expires)
    {
        var signingCredentials = new SigningCredentials(
            new RsaSecurityKey(rsaKeyProvider.Value.Rsa)
            {
                KeyId = jwtOption.Value.KeyId
            },
            SecurityAlgorithms.RsaSha256
        );

        var token = new JwtSecurityToken(
            issuer: jwtOption.Value.Issuer,
            audience: jwtOption.Value.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateAccessToken(string token)
    {
        var principal = ValidateToken(token, true);

        if (principal == null)
            return null;

        var tokenType = principal.FindFirst(StaticDetail.CLAIM_TYPE_TOKEN_TYPE)?.Value;

        return tokenType == StaticDetail.TOKEN_TYPE_ACCESS
            ? principal
            : null;
    }

    public ClaimsPrincipal? ValidateRefreshToken(string token)
    {
        var principal = ValidateToken(token, true);

        if (principal == null)
            return null;

        var tokenType = principal.FindFirst(StaticDetail.CLAIM_TYPE_TOKEN_TYPE)?.Value;

        return tokenType == StaticDetail.TOKEN_TYPE_REFRESH
            ? principal
            : null;
    }

    private ClaimsPrincipal? ValidateToken(string token, bool validateLifetime)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(jwtOption.Value.PublicKey), out _);

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOption.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOption.Value.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.Zero
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, parameters, out _);

        var jti = principal.FindFirst("jti")!.Value;
        var isRevoked = context.RevokedTokens.Any(rt => rt.Jti.ToString() == jti);
        
        if (isRevoked)
            return null;

        return principal;
    }
}
