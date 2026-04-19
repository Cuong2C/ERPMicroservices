namespace AuthService.Api.Models;

public class Claim
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;    // Employee, Invoice, Warehouse...
    public Guid ClaimValueId { get; set; }
    public ClaimValue Value { get; set; } = default!;  // read, write, approve...
    public ICollection<ClaimScope> ClaimScopes { get; set; } = new List<ClaimScope>();
}
