using Microsoft.AspNetCore.Mvc;

namespace ItemCatalog.Api.Items.GetItems;

public record GetItemsRequest(
    string? Keyword,
    IEnumerable<Guid>? CategoryIds,
    IEnumerable<Guid>? TagIds,
    int? PageNumber = 1,
    int? PageSize = 10
);

public record GetItemsResponse(
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

public static class GetItemsEndpoint 
{
    public static IEndpointRouteBuilder MapGetItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/items", async (
            [FromQuery] string? keyword,
            [FromQuery] Guid[]? categoryIds,
            [FromQuery] Guid[]? tagIds,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            ISender sender) =>
        {
            var request = new GetItemsRequest(keyword, categoryIds?.ToList(), tagIds?.ToList(), pageNumber, pageSize);
            var query = request.Adapt<GetItemsQuery>();
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetItemsResponse>();
            var result = Result<GetItemsResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithName("GetItems");

        return endpoints;
    }
}
