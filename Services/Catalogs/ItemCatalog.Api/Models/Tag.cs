using BuildingBlocks.Domain;

namespace ItemCatalog.Api.Models;

public class Tag : AuditableEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
}
