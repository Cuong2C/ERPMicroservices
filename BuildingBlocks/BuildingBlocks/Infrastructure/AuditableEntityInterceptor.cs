using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BuildingBlocks.Infrastructure;

public class AuditableEntityInterceptor(ICurrentUserService currentUserService) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var userId = currentUserService.UserId ?? "system";

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
            {
                entry.Entity.LastModifiedBy = userId;
                entry.Entity.LastModifiedAt = DateTime.UtcNow;
            }
        }
    }
}


