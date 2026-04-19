namespace ItemCatalog.Api.Apis.Units.GetUnits;

public record GetUnitsQuery(int? PageNumber = 1, int? PageSize = 10) : IRequest<GetUnitsResult>;
public record GetUnitsResult(PagedResult<UnitDto> PagedResult);

internal class GetUnitsHandler(ItemCatalogDbContext context) : IRequestHandler<GetUnitsQuery, GetUnitsResult>
{
    public async Task<GetUnitsResult> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Units.AsQueryable();

        var pageSize = Math.Min(request?.PageSize ?? 10, 100);
        var pageNumber = request?.PageNumber ?? 1;

        var units = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UnitDto(u.Id, u.Code, u.Name, u.CreatedAt, u.CreatedBy, u.LastModifiedAt, u.LastModifiedBy))
            .ToListAsync(cancellationToken);

        var totalCount = await context.Units.CountAsync(cancellationToken);

        var pagedResult = new PagedResult<UnitDto>
        {
            Data = units,
            Pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            }
        };

        return new GetUnitsResult(pagedResult);
    }
}
