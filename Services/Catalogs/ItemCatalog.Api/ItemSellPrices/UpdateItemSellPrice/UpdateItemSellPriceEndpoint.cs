namespace ItemCatalog.Api.ItemSellPrices.UpdateItemSellPrice;

public record UpdateItemSellPriceRequest(Guid Id, Guid ItemId, decimal Price, DateTime EffectiveDate);
public record UpdateItemSellPriceResponse(Guid Id);

public static class UpdateItemSellPriceEndpoint
{
    public static IEndpointRouteBuilder MapUpdateItemSellPriceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/sell-prices", async (UpdateItemSellPriceRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateItemSellPriceCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<UpdateItemSellPriceResponse>();
            var result = Result<UpdateItemSellPriceResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("ItemSellPrices")
        .WithSummary("Update item sell price")
        .WithDescription("Updates the specified sell price for an item and returns updated id.")
        .WithName("UpdateItemSellPrice");

        return endpoints;
    }
}
