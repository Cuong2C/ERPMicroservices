using AuthService.Api.Identity.Interfaces;

namespace AuthService.Api.Identity;

public class UserGuard(ICurrentUserAuthService currentUser) : IUserGuard
{
    public void EnsureCanAccess(Guid resourceUserId)
    {
        if (currentUser.IsRootAdmin || currentUser.IsAdmin)
            return;

        if (resourceUserId.ToString() == currentUser.UserId)
        {
            return;
        }

        throw new ForbiddenException("User access denied");
    }
}
