namespace AuthService.Api.Apis.OpenIdConnect.OpenIdConfiguration;

public record GetOpenIdConfigurationResponse(
    string Issuer,
    string TokenEndpoint,
    string JwksUri,
    List<string> ResponseTypesSupported,
    List<string> SubjectTypesSupported,
    List<string> IdTokenSigningAlgValuesSupported,
    List<string> ScopesSupported,
    List<string> ClaimsSupported
);

public static class GetOpenIdConfigurationEndpoint
{
    public static IEndpointRouteBuilder MapGetOpenIdConfigurationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/.well-known/openid-configuration", async (ISender sender) =>
        {
            var query = new GetOpenIdConfigurationQuery();
            var result = await sender.Send(query);
            var response = result.Adapt<GetOpenIdConfigurationResponse>();
            return Results.Ok(response);
        })
        .WithTags("OpenID Connect")
        .WithSummary("Get OpenID Connect Configuration")
        .WithDescription("Returns the OpenID Connect Discovery configuration for token validation")
        .WithName("GetOpenIdConfiguration")
        .AllowAnonymous();

        return endpoints;
    }
}
