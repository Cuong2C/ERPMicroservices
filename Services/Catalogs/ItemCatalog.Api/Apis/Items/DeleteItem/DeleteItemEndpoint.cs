using ItemCatalog.Api.Items.DeleteItem;

namespace ItemCatalog.Api.Apis.Items.DeleteItem;

public record DeleteItemResponse(bool IsSuccess);

public static class DeleteItemEndpoint
{
    public static IEndpointRouteBuilder MapDeleteItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/items/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteItemCommand(id);
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<DeleteItemResponse>();

            var result = Result<DeleteItemResponse>.Success(responseData);

            return Results.Ok(result);
        })
        .WithName("DeleteItem");

        return endpoints;
    }
}
