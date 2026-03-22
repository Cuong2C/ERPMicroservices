using BuildingBlocks.Domain;

namespace ItemCatalog.Api.Models;

public class ItemUnit : AuditableEntity
{
    public Guid ItemId { get; set; }
    public Item Item { get; set; } = default!;
    public Guid UnitId { get; set; }
    public MeasurementUnit Unit { get; set; } = default!;
    public decimal ConversionRate { get; set; } 
    public bool IsBaseUnit { get; set; }
    public string? Barcode { get; set; }
}
