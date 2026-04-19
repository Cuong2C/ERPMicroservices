namespace AuthService.Api.Models;

public class RoleClaim
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = default!;
    public Guid ClaimId { get; set; }
    public Claim Claim { get; set; } = default!;
}
