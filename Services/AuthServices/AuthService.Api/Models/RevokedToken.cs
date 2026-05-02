namespace AuthService.Api.Models;

public class RevokedToken : AuditableEntity
{
    public Guid Id { get; set; }
    public string Jti { get; set; } = default!;
}
