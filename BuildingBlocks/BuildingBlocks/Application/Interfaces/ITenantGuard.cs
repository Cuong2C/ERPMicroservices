namespace BuildingBlocks.Application.Interfaces;

public interface ITenantGuard
{
    void EnsureCanAccess(string? resourceTenantId);
}
