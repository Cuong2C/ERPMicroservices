namespace ItemCatalog.Api.Items.CreateItem;

public record CreateItemCommand(
    string Name,
    Guid BaseUnitId,
    List<ItemUnitDto> Units,
    List<Guid> CategoryIds,
    string Description,
    string ImageUrl,
    Guid TaxId,
    List<Guid> TagIds
) : IRequest<CreateItemResult>;

public record CreateItemResult(Guid Id);

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.BaseUnitId).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.ImageUrl).MaximumLength(100);
        RuleFor(x => x.TaxId).NotEmpty();
    }
}

internal class CreateItemHandler(ItemCatalogDbContext context) : IRequestHandler<CreateItemCommand, CreateItemResult>
{
    public async Task<CreateItemResult> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        // create item
        var item = new Item
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            BaseUnitId = command.BaseUnitId,
            Description = command.Description,
            ImageUrl = command.ImageUrl,
            TaxId = command.TaxId,
        };

        var categories = await context.Categories.Where(c => command.CategoryIds.Contains(c.Id)).ToListAsync(cancellationToken);

        if(categories.Count != command.CategoryIds.Count)
        {
            throw new Exception("One or more categories not found.");
        }

        item.ItemCategories = categories.Select(c => new ItemCategory { ItemId = item.Id, CategoryId = c.Id }).ToList();

        


        item.Tags = await context.Tags.Where(t => command.TagIds.Contains(t.Id)).ToListAsync(cancellationToken);

        // save item
        await context.Items.AddAsync(item, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // return result
        return new CreateItemResult(item.Id);

    }
}
