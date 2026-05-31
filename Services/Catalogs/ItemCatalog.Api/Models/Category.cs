using BuildingBlocks.Domain;

namespace ItemCatalog.Api.Models;

public class Category : AuditableEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();
}
