using ItemCatalog.Api.Data;
using ItemCatalog.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItemCatalog.Api.Items.CreateItem;

public record CreateItemCommand(
    string Name,
    string BaseUnit,
    List<Guid> CategoryIds,
    string Description,
    string ImageUrl,
    Guid TaxCodeId,
    string Barcode,
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
            BaseUnit = command.BaseUnit,
            Description = command.Description,
            ImageUrl = command.ImageUrl,
            TaxCodeId = command.TaxCodeId,
            Barcode = command.Barcode
        };

        item.Categories = await context.Categories.Where(c => command.CategoryIds.Contains(c.Id)).ToListAsync(cancellationToken);
        item.Tags = await context.Tags.Where(t => command.TagIds.Contains(t.Id)).ToListAsync(cancellationToken);

        // save item
        await context.Items.AddAsync(item, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // return result
        return new CreateItemResult(item.Id);

    }
}
