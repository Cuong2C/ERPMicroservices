namespace ItemCatalog.Api.Apis.ItemSellPrices.GetItemSellPricesByItemId;

public record GetItemSellPricesByItemIdQuery(Guid ItemId, int? PageNumber, int? PageSize) : IRequest<GetItemSellPricesByItemIdResult>;
public record GetItemSellPricesByItemIdResult(PagedResult<ItemSellPriceDto> PagedResult);

internal class GetItemSellPricesByItemIdHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<GetItemSellPricesByItemIdQuery, GetItemSellPricesByItemIdResult>
{
    public async Task<GetItemSellPricesByItemIdResult> Handle(GetItemSellPricesByItemIdQuery query, CancellationToken cancellationToken)
    {
        var item = await context.Items.FindAsync(new object[] { query.ItemId }, cancellationToken);
        if (item == null) throw new NotFoundException("Item not found.");

        tenantGuard.EnsureCanAccess(item.TenantId);

        var pageNumber = query.PageNumber ?? 1;
        var pageSize = query.PageSize ?? 10;

        var queryable = context.ItemSellPrices.Where(p => p.ItemId == query.ItemId)
            .OrderByDescending(p => p.EffectiveDate);

        var total = await queryable.CountAsync(cancellationToken);
        var itemSellPrices = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        var result = itemSellPrices.Adapt<List<ItemSellPriceDto>>();

        var paged = new PagedResult<ItemSellPriceDto>()
        {
            Data = result,
            Pagination = new Pagination
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = total
            }
        };

        return new GetItemSellPricesByItemIdResult(paged);
    }
}
