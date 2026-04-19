namespace ItemCatalog.Api.Apis.Items.UpdateItem;

public record UpdateItemRequest(
    Guid Id,
    string Code,
    string Name,
    Guid BaseUnitId,
    IEnumerable<ItemUnitDto> Units,
    IEnumerable<Guid> CategoryIds,
    string Description,
    string ImageUrl,
    decimal MinStockQuantity,
    Guid TaxId,
    IEnumerable<Guid> TagIds,
    Status? Status
);

public record ItemUnitDto(
    Guid UnitId,
    decimal ConversionRate
);

public record UpdateItemResponse(Guid Id);

public static class UpdateItemEndpoint
{
    public static IEndpointRouteBuilder MapUpdateItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/items", async (UpdateItemRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateItemCommand>(); 

            var handlerResult = await sender.Send(command);

            var responseData = handlerResult.Adapt<UpdateItemResponse>();

            var result = Result<UpdateItemResponse>.Success(responseData);

            return Results.Ok(result);
        })
        .WithName("UpdateItem");

        return endpoints;
    }
}
