namespace AuthService.Api.Apis.Tokens.IntrospectToken;

public record IntrospectTokenResponse(bool Active, string? Message);

public static class IntrospectTokenEndpoint
{
    public static IEndpointRouteBuilder MapIntrospectTokenEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/tokens/introspect", async (string jti, ISender sender) =>
        {
            var query = new IntrospectTokenQuery(jti);
            var result = await sender.Send(query);
            var response = result.Adapt<IntrospectTokenResponse>();
            return Results.Ok(response);
        })
        .WithTags("Tokens")
        .WithSummary("Introspect token")
        .WithDescription("Check if a token (by JTI) is revoked or still active")
        .WithName("IntrospectToken")
        .AllowAnonymous();

        return endpoints;
    }
}
