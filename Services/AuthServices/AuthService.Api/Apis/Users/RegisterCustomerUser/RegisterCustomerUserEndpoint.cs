namespace AuthService.Api.Apis.Users.RegisterCustomerUser;

public record RegisterCustomerUserRequest(
    string Username,
    string Password,
    string? Email,
    string? PhoneNumber
);

public record RegisterCustomerUserResponse(Guid Id);

public static class RegisterCustomerUserEndpoint
{
    public static IEndpointRouteBuilder MapRegisterCustomerUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/users/register-customer", async (RegisterCustomerUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterCustomerUserCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<RegisterCustomerUserResponse>();
            var result = Result<RegisterCustomerUserResponse>.Success(responseData);
            return Results.Created($"/users/{responseData.Id}", result);
        })
        .WithTags("Users")
        .WithSummary("Register a new customer user")
        .WithDescription("Registers a new customer user with email, password and assigned roles. Returns created user id.")
        .WithName("RegisterCustomerUser");
        return endpoints;
    }
}
