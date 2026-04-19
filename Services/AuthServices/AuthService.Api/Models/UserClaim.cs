namespace AuthService.Api.Models;

public class UserClaim
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid ClaimId { get; set; }
    public Claim Claim { get; set; } = default!;
}
