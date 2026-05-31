namespace AuthService.Api.Identity;

public class UserGuard(ICurrentUser currentUser) : IUserGuard
{
    public void EnsureCanAccess(Guid resourceUserId)
    {
        if (currentUser.IsRootAdmin)
            return;

        if (resourceUserId.ToString() == currentUser.UserId)
        {
            return;
        }

        throw new ForbiddenException("User access denied");
    }
}
