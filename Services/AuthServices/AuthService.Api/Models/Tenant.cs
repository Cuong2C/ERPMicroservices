namespace AuthService.Api.Models;

public class Tenant : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; } 
    public int? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
}
