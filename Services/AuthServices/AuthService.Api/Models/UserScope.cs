namespace AuthService.Api.Models
{
    public class UserScope
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public Guid ScopeId { get; set; }
        public Scope Scope { get; set; } = default!;
    }
}
