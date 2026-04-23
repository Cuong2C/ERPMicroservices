namespace AuthService.Api.Apis.Tenants.CreateTenant;

public record CreateTenantRequest(
    string Name,
    string Description,
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
    public static void MapCreateTenantEndpoint(this WebApplication app)
    {
        app.MapPost("/tenants", async (CreateTenantRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateTenantCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<CreateTenantResponse>();
            var result = Results.Ok(responseData);
            return Results.Created($"/tenants/{responseData.Id}", result);
        })
        .RequireAuthorization("RootAdminPolicy")
        .WithTags("Tenants");
    }
}
