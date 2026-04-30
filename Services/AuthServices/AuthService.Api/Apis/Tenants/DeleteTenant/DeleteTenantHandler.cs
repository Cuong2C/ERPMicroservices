namespace AuthService.Api.Apis.Tenants.DeleteTenant;

public record DeleteTenantCommand(Guid Id) : IRequest<DeleteTenantResult>;
public record DeleteTenantResult(Guid Id);

internal class DeleteTenantHandler(AuthServiceDbContext context, ITenantGuard tenantGuard, ICurrentUserAuthService currentUser) : IRequestHandler<DeleteTenantCommand, DeleteTenantResult>
{
    public async Task<DeleteTenantResult> Handle(DeleteTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants.FindAsync(new object[] { command.Id }, cancellationToken);
        if (tenant == null) throw new NotFoundException("Tenant not found.");

        tenantGuard.EnsureCanAccess(tenant.TenantId);

        if(!currentUser.IsAdmin && !currentUser.IsRootAdmin) 
            throw new UnauthorizedException("Only admins can delete tenants.");

        tenant.Status = Status.Deleted;

        await context.SaveChangesAsync(cancellationToken);

        return new DeleteTenantResult(tenant.Id);
    }
}
