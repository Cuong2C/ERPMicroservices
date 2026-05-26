namespace AuthService.Api.Models;

public class RevokedToken : TenantAuditableEntity
{
    public Guid Id { get; set; }
    public string Jti { get; set; } = default!;
}
