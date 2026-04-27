namespace AuthService.Api.Identity.Interfaces;

public interface ICurrentUserAuthService : ICurrentUser
{
    bool IsAdmin { get; }
}
