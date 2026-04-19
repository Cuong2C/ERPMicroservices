namespace ItemCatalog.Api.Apis.ItemSellPrices.CreateItemSellPrice;

public record CreateItemSellPriceCommand(Guid ItemId, decimal Price, DateTime EffectiveDate) : IRequest<CreateItemSellPriceResult>;
public record CreateItemSellPriceResult(Guid Id);

public class CreateItemSellPriceCommandValidator : AbstractValidator<CreateItemSellPriceCommand>
{
    public CreateItemSellPriceCommandValidator()
    {
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
    }
}

internal class CreateItemSellPriceHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<CreateItemSellPriceCommand, CreateItemSellPriceResult>
{
    public async Task<CreateItemSellPriceResult> Handle(CreateItemSellPriceCommand command, CancellationToken cancellationToken)
    {
        var item = await context.Items.FirstOrDefaultAsync(i => i.Id == command.ItemId, cancellationToken);

        if (item == null) throw new NotFoundException("Item not found.");

        tenantGuard.EnsureCanAccess(item.TenantId);

        var itemSellPrice = new ItemSellPrice
        {
            Id = Guid.NewGuid(),
            ItemId = command.ItemId,
            Price = command.Price,
            EffectiveDate = command.EffectiveDate
        };

        await context.ItemSellPrices.AddAsync(itemSellPrice, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateItemSellPriceResult(itemSellPrice.Id);
    }
}
