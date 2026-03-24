using ItemCatalog.Api.Data;
using ItemCatalog.Api.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItemCatalog.Api.Items.GetItemById;

public record GetItemByIdQuery(Guid Id) : IRequest<GetItemByIdResult>;
public record GetItemByIdResult(
    Guid Id,
    string Name,
    Guid BaseUnitId,
    List<Category> Categories,
    string Description,
    string ImageUrl,
    Guid TaxId,
    List<Tag> Tags,
    DateTime CreatedDate,
    string CreatedBy,
    DateTime ModifiedDate,
    string ModifiedBy
);
internal class GetItemByIdHandler(ItemCatalogDbContext context) : IRequestHandler<GetItemByIdQuery, GetItemByIdResult>
{
    public async Task<GetItemByIdResult> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await context.Items
            .Include(i => i.ItemCategories)
            .Include(i => i.Tags)
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (item is null)
        {
            throw new KeyNotFoundException($"Item with Id {request.Id} not found.");
        }

        var result = item.Adapt<GetItemByIdResult>();

        return result;
    }
}
