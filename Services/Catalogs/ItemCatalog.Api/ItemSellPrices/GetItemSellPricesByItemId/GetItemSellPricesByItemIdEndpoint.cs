namespace ItemCatalog.Api.ItemSellPrices.GetItemSellPrices;

public record GetItemSellPricesByItemIdRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetItemSellPricesByItemIdResponse(PagedResult<ItemSellPriceDto> PagedResult);
public record ItemSellPriceDto(Guid Id, Guid ItemId, decimal Price, DateTime EffectiveDate, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetItemSellPricesByItemIdEndpoint
{
    public static IEndpointRouteBuilder MapGetItemSellPricesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/items/{itemId:guid}/sell-prices", async (Guid itemId, [AsParameters] GetItemSellPricesByItemIdRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetItemSellPricesByItemIdQuery>() with { ItemId = itemId };
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetItemSellPricesByItemIdResponse>();
            var result = Result<GetItemSellPricesByItemIdResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("ItemSellPrices")
        .WithSummary("Retrieve item sell prices")
        .WithDescription("Returns a paged list of sell prices for the specified item.")
        .WithName("GetItemSellPrices");

        return endpoints;
    }
}
