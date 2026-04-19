using AuthService.Api.Enums;

namespace AuthService.Api.Models;

public class User : AuditableEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public Status Status { get; set; } = Status.Active; 
    public bool IsLocked { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();
    public ICollection<UserScope> UserScopes { get; set; } = new List<UserScope>();
}
