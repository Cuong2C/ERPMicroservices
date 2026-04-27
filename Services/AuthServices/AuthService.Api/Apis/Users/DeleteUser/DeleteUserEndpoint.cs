namespace AuthService.Api.Apis.Users.DeleteUser;

public record DeleteUserResponse(Guid Id);

public static class DeleteUserEndpoint
{
    public static IEndpointRouteBuilder MapDeleteUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/users/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteUserCommand(id);
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<DeleteUserResponse>();
            var result = Result<DeleteUserResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Users")
        .WithSummary("Delete user")
        .WithDescription("Deletes a user by id.")
        .WithName("DeleteUser");

        return endpoints;
    }
}
