namespace AuthService.Api.Models;

public class Scope : AuditableEntity
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;  // Department, Tenant, Warehouse... (Resource)
    public string Value { get; set; } = default!;  // HR, Finance, WH1...
}
