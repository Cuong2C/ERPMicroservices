namespace ItemCatalog.Api.Items.CreateItem;

public record CreateItemRequest(
    string Name,
    Guid BaseUnitId,
    List<ItemUnitDto> Units,
    List<Guid> CategoryIds,
    string Description,
    string ImageUrl,
    Guid TaxId,
    List<Guid> TagIds
);

public record ItemUnitDto(
    Guid UnitId,
    decimal ConversionRate
);

public record CreateItemResponse(Guid Id);

public static class CreateItemEndpoint
{
    public static IEndpointRouteBuilder MapCreateItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/items", async (CreateItemRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateItemCommand>();
            var result = await sender.Send(command);
            var reponse = result.Adapt<CreateItemResponse>();

            return Results.Created($"/items/{reponse.Id}", new CreateItemResponse(reponse.Id));
        });

        return endpoints;
    }
}
