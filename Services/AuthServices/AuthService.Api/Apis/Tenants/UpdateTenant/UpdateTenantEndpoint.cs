namespace AuthService.Api.Apis.Tenants.UpdateTenant;

public record UpdateTenantRequest(Guid Id, string Name, string Description, string? Email, string Address, string City, string? Region, int? PostalCode, string? Country, string? PhoneNumber);
public record UpdateTenantResponse(Guid Id);

public static class UpdateTenantEndpoint
{
    public static IEndpointRouteBuilder MapUpdateTenantEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/tenants", async (UpdateTenantRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateTenantCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<UpdateTenantResponse>();
            var result = Result<UpdateTenantResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Tenants")
        .WithSummary("Update tenant")
        .WithDescription("Updates an existing tenant.")
        .WithName("UpdateTenant");

        return endpoints;
    }
}
