namespace AuthService.Api.Apis.Tenants.GetTenants;

public record GetTenantRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetTenantsResponse(PagedResult<TenantDto> PagedResult);

public record TenantDto(Guid Id, string Name, string Description, string? Address, string? City, string? Region, int? PostalCode, string? Country, string? PhoneNumber, Status Status, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetTenantsEndpoint
{
    public static IEndpointRouteBuilder MapGetTenantsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/tenants", async ([AsParameters] GetTenantsQuery query, ISender sender) =>
        {
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetTenantsResponse>();
            var result = Result<GetTenantsResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Tenants")
        .WithSummary("Retrieve tenants")
        .WithDescription("Returns a paged list of tenants.")
        .WithName("GetTenants");

        return endpoints;
    }
}
