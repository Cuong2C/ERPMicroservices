namespace ItemCatalog.Api.Apis.Tags.GetTags;

public record GetTagsQuery(int? PageNumber = 1, int? PageSize = 10) : IRequest<GetTagsResult>;
public record GetTagsResult(PagedResult<TagDto> PagedResult);

internal class GetTagsHandler(ItemCatalogDbContext context) : IRequestHandler<GetTagsQuery, GetTagsResult>
{
    public async Task<GetTagsResult> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Tags.AsQueryable();

        var pageSize = Math.Min(request?.PageSize ?? 10, 100);
        var pageNumber = request?.PageNumber ?? 1;

        var tags = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TagDto(t.Id, t.Name, t.CreatedAt, t.CreatedBy, t.LastModifiedAt, t.LastModifiedBy))
            .ToListAsync(cancellationToken);

        var totalCount = await context.Tags.CountAsync(cancellationToken);

        var pagedResult = new PagedResult<TagDto>
        {
            Data = tags,
            Pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            }
        };

        return new GetTagsResult(pagedResult);
    }
}
