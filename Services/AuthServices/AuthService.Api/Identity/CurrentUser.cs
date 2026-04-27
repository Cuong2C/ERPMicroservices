using AuthService.Api.Identity.Interfaces;

namespace AuthService.Api.Identity
{
    public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUserAuthService
    {
        public string? UserId =>
        httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

        public Guid TenantId =>
             Guid.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("tenantId")?.Value ?? Guid.Empty.ToString());

        public bool IsRootAdmin =>
            httpContextAccessor.HttpContext?.User?.Claims.Any(c => c.Type == "roles" && c.Value == "RootAdmin") == true;

        public bool IsAdmin =>
            httpContextAccessor.HttpContext?.User?.Claims.Any(c => c.Type == "roles" && (c.Value == "RootAdmin" || c.Value == "Admin")) == true;
    }
}
