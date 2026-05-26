namespace BuildingBlocks.Domain;

public abstract class TenantAuditableEntity : AuditableEntity
{
    public string? TenantId { get; set; } 
}
