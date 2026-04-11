namespace ItemCatalog.Api.Units.CreateUnit;

public record CreateUnitRequest(
    string Code,
    string Name
);

public record CreateUnitResponse(Guid Id);

public static class CreateUnitEndpoint
{
    public static IEndpointRouteBuilder MapCreateUnitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/units", async (CreateUnitRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUnitCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<CreateUnitResponse>();

            var result = Result<CreateUnitResponse>.Success(responseData);

            return Results.Created($"/units/{responseData.Id}", result);
        })
        .WithTags("Units")
        .WithSummary("Create a new unit")
        .WithDescription("Creates a new measurement unit and returns created id.")
        .WithName("CreateUnit");

        return endpoints;
    }
}
