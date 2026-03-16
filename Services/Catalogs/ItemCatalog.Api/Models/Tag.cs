using BuildingBlocks.Domain;

namespace ItemCatalog.Api.Models;

public class Tag : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
