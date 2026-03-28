using ItemCatalog.Api.Models;
using Mapster;
using MediatR;

namespace ItemCatalog.Api.Items.GetItemById;

public record GetItemByIdResponse(
    Guid Id,
    string Name,
    Guid BaseUnitId,
    List<Category> Categories,
    string Description,
    string ImageUrl,
    Guid TaxId,
    List<Tag> Tags,
    Status Status,
    decimal MinStockQuantity,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime ModifiedAt,
    string LastModifiedBy
);

public static class GetItemByIdEndpoint 
{
    public static IEndpointRouteBuilder MapGetItemByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/items/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetItemByIdQuery(id);
            var result = await sender.Send(query);

            var response = result.Adapt<GetItemByIdResponse>();
            return Results.Ok(response);
        });
        return endpoints;
    }
}
