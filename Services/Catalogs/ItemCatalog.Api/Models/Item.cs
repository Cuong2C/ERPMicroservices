using BuildingBlocks.Domain;

namespace ItemCatalog.Api.Models;

public class Item : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid BaseUnitId { get; set; }
    public ICollection<ItemUnit> ItemUnits { get; set; } = new List<ItemUnit>();
    public ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public Guid TaxCodeId { get; set; }
    public List<Tag> Tags { get; set; } = new();
}
