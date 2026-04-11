namespace ItemCatalog.Api.ItemSellPrices.GetLatestItemSellPrice;

public record GetLatestItemSellPriceQuery(Guid ItemId, DateTime? Date) : IRequest<GetLatestItemSellPriceResult>;
public record GetLatestItemSellPriceResult(Guid Id, Guid ItemId, decimal Price, DateTime EffectiveDate, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

internal class GetLatestItemSellPriceHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<GetLatestItemSellPriceQuery, GetLatestItemSellPriceResult>
{
    public async Task<GetLatestItemSellPriceResult> Handle(GetLatestItemSellPriceQuery query, CancellationToken cancellationToken)
    {
        var date = query.Date ?? DateTime.UtcNow;

        var entity = await context.ItemSellPrices
            .Where(p => p.ItemId == query.ItemId && p.EffectiveDate <= date)
            .OrderByDescending(p => p.EffectiveDate)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException("Item sell price not found.");

        tenantGuard.EnsureCanAccess(entity.TenantId);

        return entity.Adapt<GetLatestItemSellPriceResult>();
    }
}
