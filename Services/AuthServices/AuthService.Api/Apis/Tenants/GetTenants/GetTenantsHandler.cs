namespace AuthService.Api.Apis.Tenants.GetTenants;

public record GetTenantsQuery(int? PageNumber = 1, int? PageSize = 10) : IRequest<GetTenantsResult>;
public record GetTenantsResult(PagedResult<TenantDto> PagedResult);


internal class GetTenantsHandler(AuthServiceDbContext context) : IRequestHandler<GetTenantsQuery, GetTenantsResult>
{
    public async Task<GetTenantsResult> Handle(GetTenantsQuery query, CancellationToken cancellationToken)
    {
        var pageNumber = query.PageNumber ?? 1;
        var pageSize = Math.Min(query.PageSize ?? 10, 100);

        var dbQuery = context.Tenants.AsQueryable();

        var total = await dbQuery.LongCountAsync(cancellationToken);

        var tenants = await dbQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TenantDto(t.Id, t.Name, t.Description, t.Address, t.City, t.Region, t.PostalCode, t.Country, t.PhoneNumber, t.Status, t.CreatedAt, t.CreatedBy, t.LastModifiedAt, t.LastModifiedBy))
            .ToListAsync(cancellationToken);

        var paged = new PagedResult<TenantDto>
        {
            Data = tenants,
            Pagination = new Pagination { PageNumber = pageNumber, PageSize = pageSize, TotalCount = total }
        };

        return new GetTenantsResult(paged);
    }
}
