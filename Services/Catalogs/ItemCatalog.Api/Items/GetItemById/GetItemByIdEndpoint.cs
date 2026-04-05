namespace ItemCatalog.Api.Items.GetItemById;

public record GetItemByIdResponse(
    Guid Id,
    string Code,
    string Name,
    Guid BaseUnitId,
    IEnumerable<UnitDto> Units,
    IEnumerable<CategoryDto> Categories,
    string Description,
    string ImageUrl,
    Guid TaxId,
    decimal MinStockQuantity,
    Status Status,
    IEnumerable<TagDto> Tags,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime LastModifiedAt,
    string LastModifiedBy
);

public record UnitDto(
    Guid Id,
    string Code,
    string Name,
    decimal ConversionRate,
    bool IsBaseUnit,
    string? Barcode
);

public record CategoryDto(
    Guid Id,
    string Code,
    string Name
);

public record TagDto(
    Guid Id,
    string Code,
    string Name
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
        })
        .WithName("GetItemById");

        return endpoints;
    }
}
