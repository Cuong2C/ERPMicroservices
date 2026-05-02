using ItemCatalog.Api.StaticDetails;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace ItemCatalog.Api.Identity;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string? UserId =>
        httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

    public string? TenantId =>
         httpContextAccessor.HttpContext?.User?.FindFirst(StaticDetail.CLAIM_TYPE_TENANT_ID)?.Value;

    public bool IsRootAdmin => 
        httpContextAccessor.HttpContext?.User?.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == StaticDetail.ROLE_ROOT_ADMIN) == true;
}
