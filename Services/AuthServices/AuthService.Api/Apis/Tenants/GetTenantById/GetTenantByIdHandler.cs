namespace AuthService.Api.Apis.Tenants.GetTenantById;

public record GetTenantByIdQuery(Guid Id) : IRequest<GetTenantByIdResult>;
public record GetTenantByIdResult(Guid Id, string Name, string Description, string? Email, string Address, string City, string? Region, int? PostalCode, string? Country, string? PhoneNumber, Status Status, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

internal class GetTenantByIdHandler(AuthServiceDbContext context, ITenantGuard tenantGuard) : IRequestHandler<GetTenantByIdQuery, GetTenantByIdResult>
{
    public async Task<GetTenantByIdResult> Handle(GetTenantByIdQuery query, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants.FindAsync(new object[] { query.Id }, cancellationToken);
        if (tenant == null) throw new NotFoundException("Tenant not found.");

        tenantGuard.EnsureCanAccess(tenant.TenantId);

        return tenant.Adapt<GetTenantByIdResult>();
    }
}
