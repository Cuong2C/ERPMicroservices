using Microsoft.Extensions.Options;

namespace AuthService.Api.Apis.OpenIdConnect.JsonWebKeySet;

public record GetJsonWebKeySetQuery : IRequest<JsonWebKeySetResult>;

public record JwkKey(
    string Kty,
    string Use,
    string Kid,
    string N,
    string E,
    string Alg
);

public record JsonWebKeySetResult(List<JwkKey> Keys);

public class GetJsonWebKeySetHandler(IOptions<JwksProvider> jwksProvider) : IRequestHandler<GetJsonWebKeySetQuery, JsonWebKeySetResult>
{
    public Task<JsonWebKeySetResult> Handle(GetJsonWebKeySetQuery request, CancellationToken cancellationToken)
    {
        var result = jwksProvider.Value.Jwks;

        return Task.FromResult(result);
    }
}
