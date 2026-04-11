namespace ItemCatalog.Api.ItemSellPrices.GetItemSellPriceById;

public record GetItemSellPriceByIdQuery(Guid Id) : IRequest<GetItemSellPriceByIdResult>;
public record GetItemSellPriceByIdResult(Guid Id, Guid ItemId, decimal Price, DateTime EffectiveDate, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

internal class GetItemSellPriceByIdHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<GetItemSellPriceByIdQuery, GetItemSellPriceByIdResult>
{
    public async Task<GetItemSellPriceByIdResult> Handle(GetItemSellPriceByIdQuery query, CancellationToken cancellationToken)
    {
        var entity = await context.Set<ItemSellPrice>().FindAsync(new object[] { query.Id }, cancellationToken);

        if (entity == null) throw new NotFoundException("Item sell price not found.");

        tenantGuard.EnsureCanAccess(entity.TenantId);

        return entity.Adapt<GetItemSellPriceByIdResult>();
    }
}
