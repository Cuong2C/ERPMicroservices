
using ItemCatalog.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItemCatalog.Api.Items.CreateItem;

public record CreateItemCommand(
    string Name,
    Guid BaseUnitId,
    List<Guid> CategoryIds,
    string Description,
    string ImageUrl,
    Guid TaxCodeId,
    List<Guid> TagIds
) : IRequest<CreateItemResult>;

public record CreateItemResult(Guid Id);

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
            TaxCodeId = command.TaxCodeId,
        };

        item.ItemCategories = await context.ItemCategories.Where(c => command.CategoryIds.Contains(c.CategoryId)).ToListAsync(cancellationToken);
        item.Tags = await context.Tags.Where(t => command.TagIds.Contains(t.Id)).ToListAsync(cancellationToken);

        // save item
        await context.Items.AddAsync(item, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // return result
        return new CreateItemResult(item.Id);

    }
}
