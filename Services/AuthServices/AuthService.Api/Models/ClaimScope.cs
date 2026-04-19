namespace AuthService.Api.Models;

public class ClaimScope
{
    public Guid ClaimId { get; set; }
    public Claim Claim { get; set; } = default!;
    public Guid ScopeId { get; set; }
    public Scope Scope { get; set; } = default!;
}
