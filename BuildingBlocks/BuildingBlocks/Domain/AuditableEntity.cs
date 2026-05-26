namespace BuildingBlocks.Domain;

public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime LastModifiedAt { get; set; }
    public string LastModifiedBy { get; set; } = default!;
}
