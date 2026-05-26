namespace AuthService.Api.Models;

public class Scope : TenantAuditableEntity
{
    public Guid Id { get; set; }
    public Guid ResourceId { get; set; } = default!;
    public Resource Resource { get; set; } = default!;
    public string Value { get; set; } = default!;
}
