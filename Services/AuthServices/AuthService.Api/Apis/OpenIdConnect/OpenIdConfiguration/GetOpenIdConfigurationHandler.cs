using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Api.Apis.OpenIdConnect.OpenIdConfiguration;

public record GetOpenIdConfigurationQuery : IRequest<OpenIdConfigurationResult>;

public record OpenIdConfigurationResult(
    string Issuer,
    string TokenEndpoint,
    string JwksUri,
    List<string> ResponseTypesSupported,
    List<string> SubjectTypesSupported,
    List<string> IdTokenSigningAlgValuesSupported,
    List<string> ScopesSupported,
    List<string> ClaimsSupported
);

public class GetOpenIdConfigurationHandler(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetOpenIdConfigurationQuery, OpenIdConfigurationResult>
{
    public Task<OpenIdConfigurationResult> Handle(GetOpenIdConfigurationQuery request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available");
        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

        var result = new OpenIdConfigurationResult(
            Issuer: jwtOptions.Value.Issuer,
            TokenEndpoint: $"{baseUrl}/users/login",
            JwksUri: $"{baseUrl}/.well-known/jwks.json",
            ResponseTypesSupported: new List<string>(),
            SubjectTypesSupported: new List<string> { "public" },
            IdTokenSigningAlgValuesSupported: new List<string> { SecurityAlgorithms.RsaSha256 },
            ScopesSupported: new List<string>(),
            ClaimsSupported: new List<string> { "sub", "iss", "aud", "exp", "iat", "nbf", "jti", "role", "permissions", "scopes" }
        );

        return Task.FromResult(result);
    }
}
