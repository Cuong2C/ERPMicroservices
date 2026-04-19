using ItemCatalog.Api.Categories.UpdateCategory;

namespace ItemCatalog.Api.Apis.Categories.UpdateCategory;

public record UpdateCategoryRequest(string Code, string Name);
public record UpdateCategoryResponse(Guid Id);

public static class UpdateCategoryEndpoint
{
    public static IEndpointRouteBuilder MapUpdateCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/categories/{id:guid}", async (Guid id, UpdateCategoryRequest request, ISender sender) =>
        {
            var command = new UpdateCategoryCommand(id, request.Code, request.Name);
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<UpdateCategoryResponse>();
            var result = Result<UpdateCategoryResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Categories")
        .WithSummary("Update a category")
        .WithDescription("Updates a category by id and returns the updated id.")
        .WithName("UpdateCategory");

        return endpoints;
    }
}
