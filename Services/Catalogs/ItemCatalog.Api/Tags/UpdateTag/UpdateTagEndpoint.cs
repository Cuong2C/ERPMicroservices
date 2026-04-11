namespace ItemCatalog.Api.Tags.UpdateTag;

public record UpdateTagRequest(string Name);
public record UpdateTagResponse(Guid Id);

public static class UpdateTagEndpoint
{
    public static IEndpointRouteBuilder MapUpdateTagEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/tags/{id:guid}", async (Guid id, UpdateTagRequest request, ISender sender) =>
        {
            var command = new UpdateTagCommand(id, request.Name);
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<UpdateTagResponse>();
            var result = Result<UpdateTagResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Tags")
        .WithSummary("Update a tag")
        .WithDescription("Updates a tag and returns the updated id.")
        .WithName("UpdateTag");

        return endpoints;
    }
}
