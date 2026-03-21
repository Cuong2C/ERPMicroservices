using ItemCatalog.Api.Models;
using Mapster;
using MediatR;

namespace ItemCatalog.Api.Items.GetItemById;

public record GetItemByIdResponse(
    Guid Id,
    string Name,
    string BaseUnit,
    List<Category> Categories,
    string Description,
    string ImageUrl,
    Guid TaxCodeId,
    string Barcode,
    List<Tag> Tags,
    DateTime CreatedDate,
    string CreatedBy,
    DateTime ModifiedDate,
    string ModifiedBy
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
