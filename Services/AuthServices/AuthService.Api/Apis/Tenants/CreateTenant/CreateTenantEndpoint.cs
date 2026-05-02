namespace AuthService.Api.Apis.Tenants.CreateTenant;

public record CreateTenantRequest(
    string Name,
    string Description,
    string? Email,
    string Address,
    string City,
    string? Region,
    int? PostalCode,
    string? Country,
    string? PhoneNumber
);

public record CreateTenantResponse(Guid Id);

public static class CreateTenantEndpoint
{
    public static IEndpointRouteBuilder MapCreateTenantEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/tenants", async (CreateTenantRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateTenantCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<CreateTenantResponse>();
            var result = Results.Ok(responseData);
            return Results.Created($"/tenants/{responseData.Id}", result);
        })
        .WithTags("Tenants")
        .WithSummary("Create a new tenant")
        .WithDescription("Creates a new tenant with the provided details.")
        .WithName("CreateTenant");

        return endpoints;
    }
}
