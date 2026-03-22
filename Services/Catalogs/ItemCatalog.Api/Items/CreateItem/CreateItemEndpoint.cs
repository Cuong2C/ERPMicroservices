namespace ItemCatalog.Api.Items.CreateItem;

public record CreateItemRequest(
    string Name,
    Guid BaseUnitId,
    List<Guid> CategoryIds,
    string Description,
    string ImageUrl,
    Guid TaxCodeId,
    List<Guid> TagIds
);

public record CreateItemResponse(Guid Id);

public static class CreateItemEndpoint
{
    public static IEndpointRouteBuilder MapCreateItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/items", async (CreateItemRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateItemCommand>();
            var result = await sender.Send(command);
            var reponse = result.Adapt<CreateItemResponse>();

            return Results.Created($"/items/{reponse.Id}", new CreateItemResponse(reponse.Id));
        });

        return endpoints;
    }
}
