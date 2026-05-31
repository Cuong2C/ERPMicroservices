namespace AuthService.Api.Identity;

public class TenantGuard(ICurrentUser currentUser) : ITenantGuard
{
    public void EnsureCanAccess(string? resourceTenantId)
    {
        if (currentUser.IsRootAdmin)
            return;

        if (resourceTenantId == currentUser.TenantId)
        {
            return;
        }

        throw new ForbiddenException("Tenant access denied");
    }
}
