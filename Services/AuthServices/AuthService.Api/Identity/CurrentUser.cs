using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthService.Api.Identity
{
    public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUserAuthService
    {
        public string? UserId =>
        httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        public Guid TenantId =>
             Guid.Parse(httpContextAccessor.HttpContext?.User?.FindFirst(StaticDetail.CLAIM_TYPE_TENANT_ID)?.Value ?? Guid.Empty.ToString());

        public bool IsRootAdmin =>
            httpContextAccessor.HttpContext?.User?.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == StaticDetail.ROLE_ROOT_ADMIN) == true;

        public bool IsAdmin =>
            httpContextAccessor.HttpContext?.User?.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == StaticDetail.ROLE_ADMIN) == true;
    }
}
