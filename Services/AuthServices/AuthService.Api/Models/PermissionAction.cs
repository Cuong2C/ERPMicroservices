namespace AuthService.Api.Models;

public class PermissionAction
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;  // read, write, approve...
}
