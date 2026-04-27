using Microsoft.AspNetCore.Mvc;

namespace ItemCatalog.Api.Apis.Items.GetDeletedItems;

public record GetDeletedItemsRequest(
    string? Keyword,
    IEnumerable<Guid>? CategoryIds,
    IEnumerable<Guid>? TagIds,
    int? PageNumber = 1,
    int? PageSize = 10
);

public record GetDeletedItemsResponse(
    PagedResult<ItemDto> PagedResult
);

public record ItemDto(
    Guid Id,
    string Code,
    string Name,
    Guid BaseUnitId,
    string Description,
    string ImageUrl,
    decimal MinStockQuantity,
    Guid TaxId,
    Status Status,
    IEnumerable<string> Categories,
    IEnumerable<string> Tags
);

public static class GetDeletedItemsEndpoint
{
    public static IEndpointRouteBuilder MapGetDeletedItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/items/deleted", async (
            [FromQuery] string? keyword,
            [FromQuery] Guid[]? categoryIds,
            [FromQuery] Guid[]? tagIds,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var request = new GetDeletedItemsRequest(keyword, categoryIds?.ToList(), tagIds?.ToList(), pageNumber, pageSize);
            var query = request.Adapt<GetDeletedItemsQuery>();
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetDeletedItemsResponse>();
            var result = Result<GetDeletedItemsResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithName("GetDeletedItems");

        return endpoints;
    }
}
