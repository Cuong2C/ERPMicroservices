using AuthService.Api.Services.JwtServices;

namespace AuthService.Api.Apis.Users.Login;

public record LoginRequest(string Username, string Password);
public record LoginResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc, DateTime RefreshTokenExpiresAtUtc);

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/users/login", async (LoginRequest request, ISender sender) =>
        {
            var command = request.Adapt<LoginCommand>();
            var handlerResult = await sender.Send(command);
            var response = handlerResult.Adapt<LoginResponse>();
            var result = Result<LoginResponse>.Success(response);
            return Results.Ok(result);
        })
        .WithTags("Users")
        .WithSummary("Login a user and receive access and refresh tokens")
        .WithDescription("login by username and password, returns an access token and a refresh token.")
        .WithName("Login");

        return endpoints;
    }
}
