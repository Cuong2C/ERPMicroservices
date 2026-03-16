using BuildingBlocks.Domain;

namespace ItemCatalog.Api.Models;

public class Item : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string BaseUnit { get; set; } = default!;
    public List<Category> Categories { get; set; } = new();
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public Guid TaxCodeId { get; set; }
    public List<Tag> Tags { get; set; } = new();
}
