namespace AuthService.Api.Identity.Interfaces;

public interface IUserGuard
{
    void EnsureCanAccess(Guid resourceUserId);
}
