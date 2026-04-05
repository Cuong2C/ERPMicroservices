namespace ItemCatalog.Api.Items.GetDeletedItems;

public record GetDeletedItemsQuery(
    string? Keyword,
    IEnumerable<Guid>? CategoryIds,
    IEnumerable<Guid>? TagIds,
    int? PageNumber = 1,
    int? PageSize = 10
) : IRequest<GetDeletedItemsResult>;

public record GetDeletedItemsResult(
    PagedResult<ItemDto> PagedResult
);

internal class GetDeletedItemsHandler(ItemCatalogDbContext context) : IRequestHandler<GetDeletedItemsQuery, GetDeletedItemsResult>
{
    public async Task<GetDeletedItemsResult> Handle(GetDeletedItemsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Items
            .Include(i => i.ItemCategories).ThenInclude(ic => ic.Category)
            .Include(i => i.Tags)
            .Where(i => i.Status == Status.Deleted)
            .AsQueryable();

        if (request is not null)
        {
            if (request.CategoryIds is not null)
            {
                query = query.Where(i => i.ItemCategories.Any(ic => request.CategoryIds.Contains(ic.CategoryId)));
            }

            if (request.TagIds is not null)
            {
                query = query.Where(i => i.Tags.Any(t => request.TagIds.Contains(t.Id)));
            }

            if(!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(i => i.Name.Contains(request.Keyword) || i.Description.Contains(request.Keyword) || i.Code.Contains(request.Keyword));
            }
        }

        var pageSize = Math.Min(request?.PageSize ?? 10, 100);
        var pageNumber = request?.PageNumber ?? 1;

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new ItemDto
            (
                i.Id,
                i.Code,
                i.Name,
                i.BaseUnitId,
                i.Description,
                i.ImageUrl,
                i.MinStockQuantity,
                i.TaxId,
                i.Status,
                i.ItemCategories.Select(ic => ic.Category.Name).ToList(),
                i.Tags.Select(t => t.Name).ToList()
            ))
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<ItemDto>
        {
            Data = items,
            Pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = items.Count
            }
        };

        return new GetDeletedItemsResult(pagedResult);
    }
}
