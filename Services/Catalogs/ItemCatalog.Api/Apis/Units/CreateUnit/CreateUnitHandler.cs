namespace ItemCatalog.Api.Apis.Units.CreateUnit;

public record CreateUnitCommand(string Code, string Name) : IRequest<CreateUnitResult>;
public record CreateUnitResult(Guid Id);

public class CreateUnitCommandValidator : AbstractValidator<CreateUnitCommand>
{
    public CreateUnitCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

internal class CreateUnitHandler(ItemCatalogDbContext context) : IRequestHandler<CreateUnitCommand, CreateUnitResult>
{
    public async Task<CreateUnitResult> Handle(CreateUnitCommand command, CancellationToken cancellationToken)
    {
        var unit = new MeasurementUnit
        {
            Code = command.Code,
            Name = command.Name
        };

        var existingUnit = await context.Units.FirstOrDefaultAsync(u => u.Code == command.Code, cancellationToken);

        if(existingUnit is not null)
        {
            throw new BadRequestException($"A unit with code '{command.Code}' already exists.");
        }

        await context.Units.AddAsync(unit, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateUnitResult(unit.Id);
    }
}
