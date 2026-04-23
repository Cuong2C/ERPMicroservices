namespace AuthService.Api.Apis.Tenants.CreateTenant;

public record CreateTenantCommand(
    string Name,
    string Description,
    string Address,
    string City,
    string? Region,
    int? PostalCode,
    string? Country,
    string? PhoneNumber
) : IRequest<CreateTenantResult>;

public record CreateTenantResult(Guid Id);

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Address).MaximumLength(500);
        RuleFor(x => x.City).MaximumLength(200);
        RuleFor(x => x.Region).MaximumLength(100).When(x => x.Region != null);
        RuleFor(x => x.Country).MaximumLength(100).When(x => x.Country != null);
        RuleFor(x => x.PhoneNumber).MaximumLength(50).When(x => x.PhoneNumber != null);
    }
}

public class CreateTenantHandler(AuthServiceDbContext context) : IRequestHandler<CreateTenantCommand, CreateTenantResult>
{
    public async Task<CreateTenantResult> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            City = request.City,
            Region = request.Region,
            PostalCode = request.PostalCode,
            Country = request.Country,
            PhoneNumber = request.PhoneNumber
        };

        tenant.TenantId = tenant.Id;

        context.Tenants.Add(tenant);
        await context.SaveChangesAsync();

        return new CreateTenantResult(tenant.Id);
    }
}
