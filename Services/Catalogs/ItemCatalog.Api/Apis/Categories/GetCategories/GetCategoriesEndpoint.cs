namespace ItemCatalog.Api.Apis.Categories.GetCategories;

public record GetCategoriesRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetCategoriesResponse(PagedResult<CategoryDto> PagedResult);
public record CategoryDto(Guid Id, string Code, string Name, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetCategoriesEndpoint
{
    public static IEndpointRouteBuilder MapGetCategoriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/categories", async ([AsParameters] GetCategoriesRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetCategoriesQuery>();
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetCategoriesResponse>();
            var result = Result<GetCategoriesResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Categories")
        .WithSummary("Retrieve categories")
        .WithDescription("Returns a paged list of categories.")
        .WithName("GetCategories");

        return endpoints;
    }
}
