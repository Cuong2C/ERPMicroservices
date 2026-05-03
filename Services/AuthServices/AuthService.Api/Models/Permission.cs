namespace AuthService.Api.Models;

public class Permission
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
    public Guid PermissionActionId { get; set; }
    public PermissionAction Action { get; set; } = default!;
    public string Code => $"{Type}.{Action.Name}".ToLower();
}
