namespace ItemCatalog.Api.Units.GetUnits;

public record GetUnitsRequest(int? PageNumber = 1, int? PageSize = 10);

public record GetUnitsResponse(PagedResult<UnitDto> PagedResult);

public record UnitDto(Guid Id, string Code, string Name, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

public static class GetUnitsEndpoint
{
    public static IEndpointRouteBuilder MapGetUnitsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/units", async ([AsParameters] GetUnitsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetUnitsQuery>();
            var handlerResult = await sender.Send(query);
            var responseData = handlerResult.Adapt<GetUnitsResponse>();
            var result = Result<GetUnitsResponse>.Success(responseData);
            return Results.Ok(result);
        })
        .WithTags("Units")
        .WithSummary("Retrieve units")
        .WithDescription("Returns a paged list of measurement units.")
        .WithName("GetUnits");

        return endpoints;
    }
}
