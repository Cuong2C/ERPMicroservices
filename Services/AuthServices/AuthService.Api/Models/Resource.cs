namespace AuthService.Api.Models;

public class Resource : TenantAuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;    // Employee, Invoice, Warehouse...
}
