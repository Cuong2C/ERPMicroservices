namespace AuthService.Api.Models;

public class ClaimValue
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;  // read, write, approve...
}
