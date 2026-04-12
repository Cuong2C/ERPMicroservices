namespace ItemCatalog.Api.ItemSellPrices.DeleteItemSellPrice;

public record DeleteItemSellPriceResponse(Guid Id);

public static class DeleteItemSellPriceEndpoint
{
    public static IEndpointRouteBuilder MapDeleteItemSellPriceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/sell-prices/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteItemSellPriceCommand(id);
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<DeleteItemSellPriceResponse>();
            var result = Result<DeleteItemSellPriceResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("ItemSellPrices")
        .WithSummary("Delete item sell price")
        .WithDescription("Deletes the specified sell price for an item and returns deleted id.")
        .WithName("DeleteItemSellPrice");

        return endpoints;
    }
}
