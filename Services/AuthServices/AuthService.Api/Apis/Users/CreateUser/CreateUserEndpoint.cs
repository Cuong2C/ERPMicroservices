namespace AuthService.Api.Apis.Users.CreateUser;

public record CreateUserRequest(
    string Username,
    string Password,
    IEnumerable<Guid> Roles,
    IEnumerable<Guid> Claims,
    IEnumerable<ScopeDto> Scopes
);

public record ScopeDto(
    string Type,
    string Value
);

public record CreateUserResponse(Guid Id);

public static class CreateUserEndpoint
{
    public static IEndpointRouteBuilder MapCreateUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/users", async (CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUserCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<CreateUserResponse>();
            var result = Result<CreateUserResponse>.Success(responseData);
            return Results.Created($"/users/{responseData.Id}", result);
        })
            .WithTags("Users")
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user with roles, claims and scopes. Returns created user id.")
            .WithName("CreateUser");
        return endpoints;
    }
}
