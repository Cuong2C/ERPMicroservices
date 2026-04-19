namespace ItemCatalog.Api.Apis.Categories.CreateCategory;

public record CreateCategoryRequest(string Code, string Name);
public record CreateCategoryResponse(Guid Id);

public static class CreateCategoryEndpoint
{
    public static IEndpointRouteBuilder MapCreateCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/categories", async (CreateCategoryRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateCategoryCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<CreateCategoryResponse>();
            var result = Result<CreateCategoryResponse>.Success(responseData);
            return Results.Created($"/categories/{responseData.Id}", result);
        })
        .WithTags("Categories")
        .WithSummary("Create a new category")
        .WithDescription("Creates a new category and returns created id.")
        .WithName("CreateCategory");

        return endpoints;
    }
}
