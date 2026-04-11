namespace ItemCatalog.Api.Categories.GetCategoryById;

public record GetCategoryByIdResponse(Guid Id, string Code, string Name, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetCategoryByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetCategoryByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/categories/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetCategoryByIdQuery(id);
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetCategoryByIdResponse>();
            var result = Result<GetCategoryByIdResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Categories")
        .WithSummary("Get category by id")
        .WithDescription("Returns category details for given id.")
        .WithName("GetCategoryById");

        return endpoints;
    }
}
