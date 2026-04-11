namespace ItemCatalog.Api.Items.CreateItem;

public record CreateItemRequest(
    string Code,
    string Name,
    Guid BaseUnitId,
    IEnumerable<ItemUnitDto> Units,
    IEnumerable<Guid> CategoryIds,
    string Description,
    string ImageUrl,
    decimal MinStockQuantity,
    Guid TaxId,
    IEnumerable<Guid> TagIds
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
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<CreateItemResponse>();

            var result = Result<CreateItemResponse>.Success(responseData);

            return Results.Created($"/items/{responseData.Id}", result);
        })
            .WithTags("Items")
            .WithSummary("Create a new item")
            .WithDescription("Creates a new item with units, categories and tags. Returns created item id.")
            .WithName("CreateItem");

        return endpoints;
    }
}
