namespace ItemCatalog.Api.Units.DeleteUnit;

public record DeleteUnitCommand(Guid Id) : IRequest<DeleteUnitResult>;
public record DeleteUnitResult(Guid Id);

internal class DeleteUnitHandler(ItemCatalogDbContext context) : IRequestHandler<DeleteUnitCommand, DeleteUnitResult>
{
    public async Task<DeleteUnitResult> Handle(DeleteUnitCommand command, CancellationToken cancellationToken)
    {
        var unit = await context.Units.FindAsync(new object[] { command.Id }, cancellationToken);

        if (unit == null) throw new NotFoundException("Unit", command.Id);

        // check usage
        var used = await context.ItemUnits.AnyAsync(iu => iu.UnitId == unit.Id, cancellationToken);
        if (used) throw new BadRequestException("Unit is in use by items and cannot be deleted.");

        context.Units.Remove(unit);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteUnitResult(unit.Id);
    }
}
