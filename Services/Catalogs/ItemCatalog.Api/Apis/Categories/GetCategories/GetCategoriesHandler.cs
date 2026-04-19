namespace ItemCatalog.Api.Apis.Categories.GetCategories;

public record GetCategoriesQuery(int? PageNumber = 1, int? PageSize = 10) : IRequest<GetCategoriesResult>;
public record GetCategoriesResult(PagedResult<CategoryDto> PagedResult);

internal class GetCategoriesHandler(ItemCatalogDbContext context) : IRequestHandler<GetCategoriesQuery, GetCategoriesResult>
{
    public async Task<GetCategoriesResult> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = context.Categories.AsQueryable();

        var pageSize = Math.Min(request?.PageSize ?? 10, 100);
        var pageNumber = request?.PageNumber ?? 1;

        var categories = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CategoryDto(c.Id, c.Code, c.Name, c.CreatedAt, c.CreatedBy, c.LastModifiedAt, c.LastModifiedBy))
            .ToListAsync(cancellationToken);

        var totalCount = await context.Categories.CountAsync(cancellationToken);

        var pagedResult = new PagedResult<CategoryDto>
        {
            Data = categories,
            Pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            }
        };

        return new GetCategoriesResult(pagedResult);
    }
}
