namespace AuthService.Api.Apis.Tenants.GetTenantById;

public record GetTenantByIdResponse(Guid Id, string Name, string Description, string Address, string City, string? Region, int? PostalCode, string? Country, string? PhoneNumber, Status Status, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetTenantByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetTenantByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/tenants/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetTenantByIdQuery(id);
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetTenantByIdResponse>();
            var result = Result<GetTenantByIdResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Tenants")
        .WithSummary("Get tenant by id")
        .WithDescription("Returns tenant details by id.")
        .WithName("GetTenantById");

        return endpoints;
    }
}
