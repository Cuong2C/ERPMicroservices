using ItemCatalog.Api.Units.DeleteUnit;

namespace ItemCatalog.Api.Apis.Units.DeleteUnit;

public record DeleteUnitResponse(Guid Id);

public static class DeleteUnitEndpoint
{
    public static IEndpointRouteBuilder MapDeleteUnitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/units/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteUnitCommand(id);
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<DeleteUnitResponse>();
            var result = Result<DeleteUnitResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Units")
        .WithSummary("Delete a unit")
        .WithDescription("Deletes a measurement unit by id and returns deleted id.")
        .WithName("DeleteUnit");

        return endpoints;
    }
}
