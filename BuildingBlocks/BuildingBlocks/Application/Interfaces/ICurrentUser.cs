namespace BuildingBlocks.Application.Interfaces;

public interface ICurrentUser
{
    string? TenantId { get; }
    string? UserId { get; }
    bool IsRootAdmin { get; }
}
