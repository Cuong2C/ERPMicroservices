using ItemCatalog.Api.Tags.DeleteTag;

namespace ItemCatalog.Api.Apis.Tags.DeleteTag;

public record DeleteTagResponse(Guid Id);

public static class DeleteTagEndpoint
{
    public static IEndpointRouteBuilder MapDeleteTagEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("/tags/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteTagCommand(id);
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<DeleteTagResponse>();
            var result = Result<DeleteTagResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Tags")
        .WithSummary("Delete a tag")
        .WithDescription("Deletes a tag by id and returns deleted id.")
        .WithName("DeleteTag");

        return endpoints;
    }
}
