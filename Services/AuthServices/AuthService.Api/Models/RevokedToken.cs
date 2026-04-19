namespace AuthService.Api.Models;

public class RevokedToken : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid Jti { get; set; }
}
