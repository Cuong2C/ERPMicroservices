namespace AuthService.Api.Models;

public class Role : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
