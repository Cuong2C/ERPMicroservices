namespace ItemCatalog.Api.ItemSellPrices.GetLatestItemSellPrice;

public record GetLatestItemSellPriceRequest(Guid ItemId, DateTime? Date = null);
public record GetLatestItemSellPriceResponse(Guid Id, Guid ItemId, decimal Price, DateTime EffectiveDate, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetLatestItemSellPriceEndpoint
{
    public static IEndpointRouteBuilder MapGetLatestItemSellPriceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/items/sell-prices/latest", async ([AsParameters] GetLatestItemSellPriceRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetLatestItemSellPriceQuery>();
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetLatestItemSellPriceResponse>();
            var result = Result<GetLatestItemSellPriceResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("ItemSellPrices")
        .WithSummary("Get latest sell price for an item")
        .WithDescription("Returns the latest sell price for the specified item where EffectiveDate <= provided date (or now).")
        .WithName("GetLatestItemSellPrice");

        return endpoints;
    }
}
