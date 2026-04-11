namespace ItemCatalog.Api.Units.GetUnitById;

public record GetUnitByIdQuery(Guid Id) : IRequest<GetUnitByIdResult>;
public record GetUnitByIdResult(Guid Id, string Code, string Name, DateTime CreatedAt, string CreatedBy, DateTime LastModifiedAt, string LastModifiedBy);

internal class GetUnitByIdHandler(ItemCatalogDbContext context) : IRequestHandler<GetUnitByIdQuery, GetUnitByIdResult>
{
    public async Task<GetUnitByIdResult> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var unit = await context.Units.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (unit is null) 
            throw new NotFoundException("MeasurementUnit", request.Id);

        return unit.Adapt<GetUnitByIdResult>();
    }
}
