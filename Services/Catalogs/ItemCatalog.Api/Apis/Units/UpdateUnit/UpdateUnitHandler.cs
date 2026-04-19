namespace ItemCatalog.Api.Apis.Units.UpdateUnit;

public record UpdateUnitCommand(Guid Id, string Code, string Name) : IRequest<UpdateUnitResult>;
public record UpdateUnitResult(Guid Id);

public class UpdateUnitCommandValidator : AbstractValidator<UpdateUnitCommand>
{
    public UpdateUnitCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

internal class UpdateUnitHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<UpdateUnitCommand, UpdateUnitResult>
{
    public async Task<UpdateUnitResult> Handle(UpdateUnitCommand command, CancellationToken cancellationToken)
    {
        var unit = await context.Units.FindAsync(new object[] { command.Id }, cancellationToken);

        if (unit is null) throw new NotFoundException("Unit", command.Id);

        tenantGuard.EnsureCanAccess(unit.TenantId);

        unit.Code = command.Code;
        unit.Name = command.Name;

        context.Units.Update(unit);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateUnitResult(unit.Id);
    }
}
