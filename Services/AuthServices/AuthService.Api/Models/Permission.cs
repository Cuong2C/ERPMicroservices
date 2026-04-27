namespace AuthService.Api.Models;

public class Permission
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;    // Employee, Invoice, Warehouse...
    public Guid PermissionActionId { get; set; }
    public PermissionAction Action { get; set; } = default!;  // read, write, approve...
    public string Code => $"{Type}.{Action.Name}".ToLower();
}
