namespace ItemCatalog.Api.Apis.Units.GetUnitById;

public record GetUnitByIdResponse(Guid Id, string Code, string Name, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetUnitByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetUnitByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/units/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetUnitByIdQuery(id);
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetUnitByIdResponse>();
            var result = Result<GetUnitByIdResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Units")
        .WithSummary("Get unit by id")
        .WithDescription("Returns details of a measurement unit.")
        .WithName("GetUnitById");

        return endpoints;
    }
}
