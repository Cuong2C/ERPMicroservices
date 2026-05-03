namespace AuthService.Api.Apis.Users.RegisterTenantUser;
public record RegisterTenantUserRequest(
    string Username,
    string Password,
    string? Email,
    string? PhoneNumber,
    Guid TenantId
);

public record RegisterTenantUserResponse(Guid Id);

public static class RegisterTenantUserEndpoint
{
    public static IEndpointRouteBuilder MapRegisterTenantUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/users/register-tenant-user", async (RegisterTenantUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterTenantUserCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<RegisterTenantUserResponse>();
            var result = Result<RegisterTenantUserResponse>.Success(responseData);
            return Results.Created($"/users/{responseData.Id}", result);
        })
        .WithTags("Users")
        .WithSummary("Register a new tenant user")
        .WithDescription("Registers a new tenant user with email, password and assigned roles. Returns created user id.")
        .WithName("RegisterTenantUser");
        return endpoints;
    }
}
