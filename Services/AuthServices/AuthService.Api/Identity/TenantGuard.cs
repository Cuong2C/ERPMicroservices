using BuildingBlocks.Application.Interfaces;

namespace AuthService.Api.Identity
{
    public class TenantGuard(ICurrentUser currentUser) : ITenantGuard
    {
        public void EnsureCanAccess(Guid? resourceTenantId)
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
}
