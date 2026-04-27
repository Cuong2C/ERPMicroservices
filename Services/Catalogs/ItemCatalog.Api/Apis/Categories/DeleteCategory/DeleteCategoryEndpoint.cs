namespace ItemCatalog.Api.Apis.Categories.DeleteCategory;

public record DeleteCategoryResponse(Guid Id);

public static class DeleteCategoryEndpoint
{
    public static IEndpointRouteBuilder MapDeleteCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/categories/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteCategoryCommand(id);
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<DeleteCategoryResponse>();
            var result = Result<DeleteCategoryResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Categories")
        .WithSummary("Delete a category")
        .WithDescription("Deletes a category by id and returns the deleted id.")
        .WithName("DeleteCategory");

        return endpoints;
    }
}
