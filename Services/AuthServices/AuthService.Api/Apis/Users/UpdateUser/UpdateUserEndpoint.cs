namespace AuthService.Api.Apis.Users.UpdateUser;

public record UpdateUserRequest(Guid Id, string Username, Status Status, IEnumerable<Guid> Roles, IEnumerable<Guid> Claims, IEnumerable<ScopeDto> Scopes);
public record UpdateUserResponse(Guid Id);

public record ScopeDto(string Type, string Value);

public static class UpdateUserEndpoint
{
    public static IEndpointRouteBuilder MapUpdateUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/users", async (UpdateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateUserCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<UpdateUserResponse>();
            var result = Result<UpdateUserResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Users")
        .WithSummary("Update user")
        .WithDescription("Updates an existing user with roles, claims and scopes.")
        .WithName("UpdateUser");

        return endpoints;
    }
}
