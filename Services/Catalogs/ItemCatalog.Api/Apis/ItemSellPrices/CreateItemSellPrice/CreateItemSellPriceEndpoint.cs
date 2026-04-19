namespace ItemCatalog.Api.Apis.ItemSellPrices.CreateItemSellPrice;

public record CreateItemSellPriceRequest(Guid ItemId, decimal Price, DateTime EffectiveDate);
public record CreateItemSellPriceResponse(Guid Id);

public static class CreateItemSellPriceEndpoint
{
    public static IEndpointRouteBuilder MapCreateItemSellPriceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/sell-prices", async (CreateItemSellPriceRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateItemSellPriceCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<CreateItemSellPriceResponse>();
            var result = Result<CreateItemSellPriceResponse>.Success(responseData);
            return Results.Created($"/sell-prices/{responseData.Id}", result);
        })
        .WithTags("ItemSellPrices")
        .WithSummary("Create item sell price")
        .WithDescription("Creates a new sell price for the specified item and returns created id.")
        .WithName("CreateItemSellPrice");

        return endpoints;
    }
}
