using BuildingBlocks.Application.Interfaces;

namespace AuthService.Api.Identity
{
    public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        public string? UserId =>
        httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

        public Guid TenantId =>
             Guid.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("tenantId")?.Value ?? Guid.Empty.ToString());

        public bool IsRootAdmin =>
            httpContextAccessor.HttpContext?.User?.Claims.Any(c => c.Type == "roles" && c.Value == "RootAdmin") == true;
    }
}
