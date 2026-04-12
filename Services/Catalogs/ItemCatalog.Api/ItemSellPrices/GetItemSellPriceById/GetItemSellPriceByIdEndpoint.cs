namespace ItemCatalog.Api.ItemSellPrices.GetItemSellPriceById;

public record GetItemSellPriceByIdResponse(Guid Id, Guid ItemId, decimal Price, DateTime EffectiveDate, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetItemSellPriceByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetItemSellPriceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/sell-prices/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetItemSellPriceByIdQuery(id);
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetItemSellPriceByIdResponse>();
            var result = Result<GetItemSellPriceByIdResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("ItemSellPrices")
        .WithSummary("Get sell price by id")
        .WithDescription("Returns a sell price record for the specified item and id.")
        .WithName("GetItemSellPrice");

        return endpoints;
    }
}
