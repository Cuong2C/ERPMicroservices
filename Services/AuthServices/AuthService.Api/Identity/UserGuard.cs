namespace AuthService.Api.Identity;

public class UserGuard(ICurrentUserAuthService currentUser) : IUserGuard
{
    public void EnsureCanAccess(Guid resourceUserId)
    {
        if (currentUser.IsRootAdmin || currentUser.IsShopOwner)
            return;

        if (resourceUserId.ToString() == currentUser.UserId)
        {
            return;
        }

        throw new ForbiddenException("User access denied");
    }
}
