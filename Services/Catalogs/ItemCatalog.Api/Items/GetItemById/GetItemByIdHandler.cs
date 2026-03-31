namespace ItemCatalog.Api.Items.GetItemById;

public record GetItemByIdQuery(Guid Id) : IRequest<GetItemByIdResult>;
public record GetItemByIdResult(
    Guid Id,
    string Code,
    string Name,
    Guid BaseUnitId,
    List<ItemUnit> ItemUnits,
    List<ItemCategory> ItemCategories,
    string Description,
    string ImageUrl,
    Guid TaxId,
    decimal MinStockQuantity,
    Status Status,
    List<Tag> Tags,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime LastModifiedAt,
    string LastModifiedBy
);
internal class GetItemByIdHandler(ItemCatalogDbContext context) : IRequestHandler<GetItemByIdQuery, GetItemByIdResult>
{
    public async Task<GetItemByIdResult> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await context.Items
            .Include(i => i.ItemUnits)
            .Include(i => i.ItemCategories)
            .Include(i => i.Tags)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (item is null)
        {
            throw new KeyNotFoundException($"Item with Id {request.Id} not found.");
        }

        var result = item.Adapt<GetItemByIdResult>();

        return result;
    }
}
