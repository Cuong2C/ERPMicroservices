using BuildingBlocks.Domain;
using ItemCatalog.Api.Enums;

namespace ItemCatalog.Api.Models;

public class Item : AuditableEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public Guid BaseUnitId { get; set; }
    public ICollection<ItemUnit> ItemUnits { get; set; } = new List<ItemUnit>();
    public ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public Guid TaxId { get; set; }
    public Status Status { get; set; } = Status.Active;
    public decimal MinStockQuantity { get; set; }
    public List<Tag> Tags { get; set; } = new();

}
