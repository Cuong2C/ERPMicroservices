using ItemCatalog.Api.Tags.GetTagById;

namespace ItemCatalog.Api.Apis.Tags.GetTagById;

public record GetTagByIdResponse(Guid Id, string Name, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetTagByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetTagByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/tags/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetTagByIdQuery(id);
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetTagByIdResponse>();
            var result = Result<GetTagByIdResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Tags")
        .WithSummary("Get tag by id")
        .WithDescription("Returns details of a tag.")
        .WithName("GetTagById");

        return endpoints;
    }
}
