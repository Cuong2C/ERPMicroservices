namespace AuthService.Api.Apis.Tenants.UpdateTenant;

public record UpdateTenantCommand(Guid Id, string Name, string Description, string Address, string City, string? Region, int? PostalCode, string? Country, string? PhoneNumber) : IRequest<UpdateTenantResult>;
public record UpdateTenantResult(Guid Id);

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

internal class UpdateTenantHandler(AuthServiceDbContext context, ITenantGuard tenantGuard, IUserGuard userGuard) : IRequestHandler<UpdateTenantCommand, UpdateTenantResult>
{
    public async Task<UpdateTenantResult> Handle(UpdateTenantCommand command, CancellationToken cancellationToken)
    {
        var tenant = await context.Tenants.FindAsync(new object[] { command.Id }, cancellationToken);
        if (tenant == null) throw new NotFoundException("Tenant not found.");

        tenantGuard.EnsureCanAccess(tenant.TenantId);

        tenant.Name = command.Name;
        tenant.Description = command.Description;
        tenant.Address = command.Address;
        tenant.City = command.City;
        tenant.Region = command.Region;
        tenant.PostalCode = command.PostalCode;
        tenant.Country = command.Country;
        tenant.PhoneNumber = command.PhoneNumber;

        context.Tenants.Update(tenant);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateTenantResult(tenant.Id);
    }
}
