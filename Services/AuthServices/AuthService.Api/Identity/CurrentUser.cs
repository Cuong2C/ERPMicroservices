using BuildingBlocks.Application.Interfaces;
using System.Security.Claims;

namespace AuthService.Api.Identity;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string? UserId =>
        httpContextAccessor.HttpContext?.Request.Headers[StaticDetail.USER_ID_HEADER];

    public string? TenantId =>
        httpContextAccessor.HttpContext?.Request.Headers[StaticDetail.TENANT_ID_HEADER];

    public bool IsRootAdmin =>
        httpContextAccessor.HttpContext?.User?.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == StaticDetail.ROLE_ROOT_ADMIN) == true;
}
