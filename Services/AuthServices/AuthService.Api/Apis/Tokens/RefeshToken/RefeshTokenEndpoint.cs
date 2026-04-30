namespace AuthService.Api.Apis.Tokens.RefeshToken;

public record RefeshTokenRequest(string RefreshToken);

public record RefeshTokenResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc, DateTime RefreshTokenExpiresAtUtc);

public static class RefeshTokenEndpoint
{
    public static IEndpointRouteBuilder MapRefeshTokenEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/tokens/refresh", async (RefeshTokenRequest request, ISender sender) =>
        {
            var command = request.Adapt<RefeshTokenCommand>();
            var handlerResult = await sender.Send(command);
            var response = handlerResult.Adapt<RefeshTokenResponse>();
            var result = Result<RefeshTokenResponse>.Success(response);
            return Results.Ok(result);
        })
        .WithTags("Tokens")
        .WithSummary("Refresh access token using a refresh token")
        .WithDescription("Provide a valid refresh token to receive a new access token and refresh token.")
        .WithName("RefreshToken");
        return endpoints;
    }
}
