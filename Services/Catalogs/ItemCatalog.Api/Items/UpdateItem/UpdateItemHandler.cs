using ItemCatalog.Api.Services;

namespace ItemCatalog.Api.Items.UpdateItem;

public record UpdateItemCommand(
    Guid Id,
    string Code,
    string Name,
    Guid BaseUnitId,
    IEnumerable<ItemUnitDto> Units,
    IEnumerable<Guid> CategoryIds,
    string Description,
    string ImageUrl,
    decimal MinStockQuantity,
    Guid TaxId,
    IEnumerable<Guid> TagIds,
    Status? Status
) : IRequest<UpdateItemResult>;

public record UpdateItemResult(Guid Id);

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Item ID is required");
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50).WithMessage("Item code is required");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200).WithMessage("Item name is require");
        RuleFor(x => x.BaseUnitId).NotEmpty().WithMessage("Base unit is required");
        RuleFor(x => x.TaxId).NotEmpty().WithMessage("Tax is required");
    }
}

internal class UpdateItemHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<UpdateItemCommand, UpdateItemResult>
{
    public async Task<UpdateItemResult> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var item = await context.Items
            .Include(i => i.ItemUnits)
            .Include(i => i.ItemCategories)
            .Include(i => i.Tags)
            .FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken);

        if (item == null)
            throw new NotFoundException("Item not found.");

        tenantGuard.EnsureCanAccess(item.TenantId);

        item.Code = command.Code;
        item.Name = command.Name;
        item.BaseUnitId = command.BaseUnitId;
        item.Description = command.Description;
        item.ImageUrl = command.ImageUrl;
        item.TaxId = command.TaxId;
        item.MinStockQuantity = command.MinStockQuantity;
        item.Status = command.Status ?? item.Status;

        // validate categories
        var categories = await context.Categories.Where(c => command.CategoryIds.Contains(c.Id)).ToListAsync(cancellationToken);

        if (categories.Count != command.CategoryIds.Count())
        {
            throw new NotFoundException("One or more categories not found.");
        }

        // replace item categories
        item.ItemCategories.Clear();
        item.ItemCategories = categories.Select(c => new ItemCategory { ItemId = item.Id, CategoryId = c.Id }).ToList();

        // validate units
        var unitIds = command.Units.Select(u => u.UnitId).ToList();

        if (!unitIds.Contains(command.BaseUnitId))
        {
            unitIds.Add(command.BaseUnitId);
        }

        var unitsInDb = await context.Units.Where(u => unitIds.Contains(u.Id)).ToListAsync(cancellationToken);

        if (unitsInDb.Count != unitIds.Count)
        {
            throw new NotFoundException("One or more units not found.");
        }

        // replace item units
        item.ItemUnits.Clear();
        item.ItemUnits = command.Units.Select(u => new ItemUnit
        {
            ItemId = item.Id,
            UnitId = u.UnitId,
            ConversionRate = u.ConversionRate,
            IsBaseUnit = u.UnitId == command.BaseUnitId
        }).ToList();

        // replace tags
        item.Tags = await context.Tags.Where(t => command.TagIds.Contains(t.Id)).ToListAsync(cancellationToken);

        context.Items.Update(item);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateItemResult(item.Id);
    }
}
