namespace ItemCatalog.Api.Items.CreateItem;

public record CreateItemCommand(
    string Code,
    string Name,
    Guid BaseUnitId,
    List<ItemUnitDto> Units,
    List<Guid> CategoryIds,
    string Description,
    string ImageUrl,
    Guid TaxId,
    decimal MinStockQuantity,
    List<Guid> TagIds
) : IRequest<CreateItemResult>;

public record CreateItemResult(Guid Id);

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(50).WithMessage("Item code is required");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200).WithMessage("Item name is require");
        RuleFor(x => x.BaseUnitId).NotEmpty().WithMessage("Base unit is required");
        RuleFor(x => x.TaxId).NotEmpty().WithMessage("Tax is required");
    }
}

internal class CreateItemHandler(ItemCatalogDbContext context) : IRequestHandler<CreateItemCommand, CreateItemResult>
{
    public async Task<CreateItemResult> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        // create item
        var item = new Item
        {
            Code = command.Code,
            Name = command.Name,
            BaseUnitId = command.BaseUnitId,
            Description = command.Description,
            ImageUrl = command.ImageUrl,
            TaxId = command.TaxId,
            Status = Status.Active,
            MinStockQuantity = command.MinStockQuantity
        };

        // validate categories
        var categories = await context.Categories.Where(c => command.CategoryIds.Contains(c.Id)).ToListAsync(cancellationToken);

        if(categories.Count != command.CategoryIds.Count)
        {
            throw new Exception("One or more categories not found.");
        }

        item.ItemCategories = categories.Select(c => new ItemCategory { ItemId = item.Id, CategoryId = c.Id }).ToList();

        // validate units
        var unitIds = command.Units.Select(u => u.UnitId).ToList();

        // Ensure base unit is included in the units list
        if (!unitIds.Contains(command.BaseUnitId))
        {
            unitIds.Add(command.BaseUnitId);
        }

        var unitsInDb = await context.Units.Where(u => unitIds.Contains(u.Id)).ToListAsync(cancellationToken);

        if(unitsInDb.Count != command.Units.Count)
        {
            throw new Exception("One or more units not found.");
        }

        item.ItemUnits = command.Units.Select(u => new ItemUnit
        {
            ItemId = item.Id,
            UnitId = u.UnitId,
            ConversionRate = u.ConversionRate
        }).ToList();

        item.Tags = await context.Tags.Where(t => command.TagIds.Contains(t.Id)).ToListAsync(cancellationToken);

        // save item
        await context.Items.AddAsync(item, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // return result
        return new CreateItemResult(item.Id);

    }
}
