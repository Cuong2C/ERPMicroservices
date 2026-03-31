namespace ItemCatalog.Api.Items.GetItemById;

public record GetItemByIdResponse(
    Guid Id,
    string Code,
    string Name,
    Guid BaseUnitId,
    List<ItemUnit> ItemUnits,
    List<ItemCategory> ItemCategories,
    string Description,
    string ImageUrl,
    Guid TaxId,
    List<Tag> Tags,
    Status Status,
    decimal MinStockQuantity,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime LastModifiedAt,
    string LastModifiedBy
);

public static class GetItemByIdEndpoint 
{
    public static IEndpointRouteBuilder MapGetItemByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/items/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetItemByIdQuery(id);
            var handlerResult = await sender.Send(query);

            var responseData = handlerResult.Adapt<GetItemByIdResponse>();

            var response = Result<GetItemByIdResponse>.Success(responseData);
            return Results.Ok(response);
        });

        return endpoints;
    }
}
