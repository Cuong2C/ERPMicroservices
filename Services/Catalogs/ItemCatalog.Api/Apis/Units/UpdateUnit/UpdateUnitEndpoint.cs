using ItemCatalog.Api.Units.UpdateUnit;

namespace ItemCatalog.Api.Apis.Units.UpdateUnit;

public record UpdateUnitRequest(string Code, string Name);
public record UpdateUnitResponse(Guid Id);

public static class UpdateUnitEndpoint
{
    public static IEndpointRouteBuilder MapUpdateUnitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/units/{id:guid}", async (Guid id, UpdateUnitRequest request, ISender sender) =>
        {
            var command = new UpdateUnitCommand(id, request.Code, request.Name);
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<UpdateUnitResponse>();
            var result = Result<UpdateUnitResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Units")
        .WithSummary("Update a unit")
        .WithDescription("Updates a measurement unit and returns the updated id.")
        .WithName("UpdateUnit");

        return endpoints;
    }
}
