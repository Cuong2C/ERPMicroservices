namespace AuthService.Api.Apis.Tenants.DeleteTenant;

public record DeleteTenantResponse(Guid Id);

public static class DeleteTenantEndpoint
{
    public static IEndpointRouteBuilder MapDeleteTenantEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/tenants/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteTenantCommand(id);
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<DeleteTenantResponse>();
            var result = Result<DeleteTenantResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Tenants")
        .WithSummary("Delete tenant")
        .WithDescription("Deletes a tenant by id.")
        .WithName("DeleteTenant");

        return endpoints;
    }
}
