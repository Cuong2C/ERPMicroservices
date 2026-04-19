namespace ItemCatalog.Api.Apis.ItemSellPrices.DeleteItemSellPrice;

public record DeleteItemSellPriceCommand(Guid Id) : IRequest<DeleteItemSellPriceResult>;
public record DeleteItemSellPriceResult(Guid Id);

internal class DeleteItemSellPriceHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<DeleteItemSellPriceCommand, DeleteItemSellPriceResult>
{
    public async Task<DeleteItemSellPriceResult> Handle(DeleteItemSellPriceCommand command, CancellationToken cancellationToken)
    {
        var entity = await context.ItemSellPrices.FindAsync(new object[] { command.Id }, cancellationToken);

        if (entity == null) throw new NotFoundException("Item sell price not found.");

        tenantGuard.EnsureCanAccess(entity.TenantId);

        context.ItemSellPrices.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteItemSellPriceResult(entity.Id);
    }
}
