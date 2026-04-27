namespace AuthService.Api.Apis.Users.GetUsers;

public record GetUsersRequest(int? PageNumber = 1, int? PageSize = 10, string? Keyword = null);

public record GetUsersResponse(PagedResult<UserDto> PagedResult);

public record UserDto(Guid Id, string Username, Status Status, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetUsersEndpoint
{
    public static IEndpointRouteBuilder MapGetUsersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/users", async ([AsParameters] GetUsersRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetUsersQuery>();
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetUsersResponse>();
            var result = Result<GetUsersResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Users")
        .WithSummary("Retrieve users")
        .WithDescription("Returns a paged list of users.")
        .WithName("GetUsers");

        return endpoints;
    }
}
