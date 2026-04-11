namespace BuildingBlocks.Application.Interfaces;

public interface ICurrentUser
{
    Guid TenantId { get; }
    string? UserId { get; }
    bool IsRootAdmin { get; }
}
