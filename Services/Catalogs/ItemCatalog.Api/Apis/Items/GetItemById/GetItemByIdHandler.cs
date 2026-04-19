namespace ItemCatalog.Api.Apis.Items.GetItemById;

public record GetItemByIdQuery(Guid Id) : IRequest<GetItemByIdResult>;
public record GetItemByIdResult(
    Guid Id,
    string Code,
    string Name,
    Guid BaseUnitId,
    IEnumerable<UnitDto> Units,
    IEnumerable<CategoryDto> Categories,
    string Description,
    string ImageUrl,
    Guid TaxId,
    decimal MinStockQuantity,
    Status Status,
    IEnumerable<TagDto> Tags,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime LastModifiedAt,
    string LastModifiedBy
);
internal class GetItemByIdHandler(ItemCatalogDbContext context) : IRequestHandler<GetItemByIdQuery, GetItemByIdResult>
{
    public async Task<GetItemByIdResult> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Items
            .Where(i => i.Id == request.Id)
            .Select(i => new GetItemByIdResult(
                i.Id,
                i.Code,
                i.Name,
                i.BaseUnitId,
                i.ItemUnits.Select(x => new UnitDto(
                    x.Unit.Id,
                    x.Unit.Code,
                    x.Unit.Name,
                    x.ConversionRate,
                    x.IsBaseUnit,
                    x.Barcode)),
                i.ItemCategories.Select(x => new CategoryDto(x.Category.Id, x.Category.Code, x.Category.Name)),
                i.Description,
                i.ImageUrl,
                i.TaxId,
                i.MinStockQuantity,
                i.Status,
                i.Tags.Select(x => new TagDto(x.Id, x.Code, x.Name)),
                i.CreatedAt,
                i.CreatedBy,
                i.LastModifiedAt,
                i.LastModifiedBy
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            throw new NotFoundException("Item", request.Id);
        }

        return result;
    }
}
