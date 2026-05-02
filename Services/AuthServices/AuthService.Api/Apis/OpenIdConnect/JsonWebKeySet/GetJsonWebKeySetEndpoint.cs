namespace AuthService.Api.Apis.OpenIdConnect.JsonWebKeySet;

public record JwkKeyResponse(
    string Kty,
    string Use,
    string Kid,
    string N,
    string E,
    string Alg
);

public record GetJsonWebKeySetResponse(List<JwkKeyResponse> Keys);

public static class GetJsonWebKeySetEndpoint
{
    public static IEndpointRouteBuilder MapGetJsonWebKeySetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/.well-known/jwks.json", async (ISender sender) =>
        {
            var query = new GetJsonWebKeySetQuery();
            var result = await sender.Send(query);
            var response = result.Adapt<GetJsonWebKeySetResponse>();
            return Results.Ok(response);
        })
        .WithTags("OpenID Connect")
        .WithSummary("Get JSON Web Key Set")
        .WithDescription("Returns the JSON Web Key Set (JWKS) for token validation")
        .WithName("GetJsonWebKeySet")
        .AllowAnonymous();

        return endpoints;
    }
}
