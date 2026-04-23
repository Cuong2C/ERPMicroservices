namespace AuthService.Api.Apis.Users.GetUserById;

public record GetUserByIdResponse(Guid Id, string Username, Status Status, bool IsLocked, DateTime? LockoutEnd, int AccessFailedCount, IEnumerable<RoleDto> Roles, IEnumerable<ClaimDto>? Claims, IEnumerable<ScopeDto>? Scopes, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public record RoleDto(Guid Id, string Name);
public record ClaimDto(Guid Id, string Type, ClaimValue Value);
public record ScopeDto(Guid Id, string Type, string Value);
public static class GetUserByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetUserByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/users/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetUserByIdQuery(id);
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetUserByIdResponse>();
            var result = Result<GetUserByIdResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Users")
        .WithSummary("Get user by id")
        .WithDescription("Returns user details with roles, claims and scopes.")
        .WithName("GetUserById");

        return endpoints;
    }
}
