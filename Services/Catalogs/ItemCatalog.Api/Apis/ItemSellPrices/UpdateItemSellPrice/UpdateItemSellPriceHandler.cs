namespace ItemCatalog.Api.Apis.ItemSellPrices.UpdateItemSellPrice;

public record UpdateItemSellPriceCommand(Guid Id, Guid ItemId, decimal Price, DateTime EffectiveDate) : IRequest<UpdateItemSellPriceResult>;
public record UpdateItemSellPriceResult(Guid Id);

public class UpdateItemSellPriceCommandValidator : AbstractValidator<UpdateItemSellPriceCommand>
{
    public UpdateItemSellPriceCommandValidator()
    {
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
    }
}

internal class UpdateItemSellPriceHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<UpdateItemSellPriceCommand, UpdateItemSellPriceResult>
{
    public async Task<UpdateItemSellPriceResult> Handle(UpdateItemSellPriceCommand command, CancellationToken cancellationToken)
    {
        var entity = await context.ItemSellPrices.FindAsync(new object[] { command.Id }, cancellationToken);

        if (entity == null) throw new NotFoundException("Item sell price not found.");

        tenantGuard.EnsureCanAccess(entity.TenantId);

        entity.ItemId = command.ItemId;
        entity.Price = command.Price;
        entity.EffectiveDate = command.EffectiveDate;

        context.ItemSellPrices.Update(entity);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateItemSellPriceResult(entity.Id);
    }
}
