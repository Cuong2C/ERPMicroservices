using BuildingBlocks.Domain;

namespace ItemCatalog.Api.Models;

public class ItemSellPrice : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public decimal Price { get; set; }
    public DateTime EffectiveDate { get; set; }
}
