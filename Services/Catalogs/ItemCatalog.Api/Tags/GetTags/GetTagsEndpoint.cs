namespace ItemCatalog.Api.Tags.GetTags;

public record GetTagsRequest(int? PageNumber = 1, int? PageSize = 10);
public record GetTagsResponse(PagedResult<TagDto> PagedResult);
public record TagDto(Guid Id, string Name, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetTagsEndpoint
{
    public static IEndpointRouteBuilder MapGetTagsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/tags", async ([AsParameters] GetTagsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetTagsQuery>();
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetTagsResponse>();
            var result = Result<GetTagsResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Tags")
        .WithSummary("Retrieve tags")
        .WithDescription("Returns a paged list of tags.")
        .WithName("GetTags");

        return endpoints;
    }
}
