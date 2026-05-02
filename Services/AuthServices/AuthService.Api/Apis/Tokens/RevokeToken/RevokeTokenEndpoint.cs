namespace AuthService.Api.Apis.Tokens.RevokeToken;

public record RevokeTokenRequest(string Jti);

public record RevokeTokenResponse(bool IsSuccess);

public static class RevokeTokenEndpoint
{
    public static IEndpointRouteBuilder MapRevokeTokenEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/tokens/revoke", async (RevokeTokenRequest request, ISender sender) =>
        {
            var command = request.Adapt<RevokeTokenCommand>();
            var handlerResult = await sender.Send(command);
            var response = handlerResult.Adapt<RevokeTokenResponse>();
            var result = Result<RevokeTokenResponse>.Success(response);
            return Results.Ok(result);
        })
        .WithTags("Tokens")
        .WithSummary("Revoke a token")
        .WithDescription("Provide the JTI of the token to revoke it.")
        .WithName("RevokeToken");
        return endpoints;
    }
}
