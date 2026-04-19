namespace AuthService.Api.Apis.Users.Register;
public record RegisterUserRequest(
    string Username,
    string Password,
    Guid TenantId
);

public record RegisterUserResponse(Guid Id);

public static class RegisterUserEndpoint
{
    public static IEndpointRouteBuilder MapRegisterEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/users/register", async (RegisterUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterUserCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<RegisterUserResponse>();
            var result = Result<RegisterUserResponse>.Success(responseData);
            return Results.Created($"/users/{responseData.Id}", result);
        })
            .WithTags("Users")
            .WithSummary("Register a new user")
            .WithDescription("Registers a new user with email, password and assigned roles. Returns created user id.")
            .WithName("RegisterUser");
        return endpoints;
    }
}
