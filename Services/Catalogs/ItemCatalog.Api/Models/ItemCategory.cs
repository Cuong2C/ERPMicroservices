using BuildingBlocks.Domain;

namespace ItemCatalog.Api.Models;

public class ItemCategory : AuditableEntity
{
    public Guid ItemId { get; set; }
    public Item Item { get; set; } = default!;
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}
