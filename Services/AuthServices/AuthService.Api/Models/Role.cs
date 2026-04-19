namespace AuthService.Api.Models;

public class Role : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
}
