namespace ItemCatalog.Api.Tags.CreateTag;

public record CreateTagRequest(string Name);
public record CreateTagResponse(Guid Id);

public static class CreateTagEndpoint
{
    public static IEndpointRouteBuilder MapCreateTagEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/tags", async (CreateTagRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateTagCommand>();
            var handlerResult = await sender.Send(command);
            var responseData = handlerResult.Adapt<CreateTagResponse>();
            var result = Result<CreateTagResponse>.Success(responseData);
            return Results.Created($"/tags/{responseData.Id}", result);
        })
        .WithTags("Tags")
        .WithSummary("Create a new tag")
        .WithDescription("Creates a new tag and returns created id.")
        .WithName("CreateTag");

        return endpoints;
    }
}
