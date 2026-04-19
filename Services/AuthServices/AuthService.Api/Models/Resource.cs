namespace AuthService.Api.Models;

public class Resource : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;    // Employee, Invoice, Warehouse...
}
