using BuildingBlocks.Application.Interfaces;

namespace ItemCatalog.Api.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId =>
        httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
}
