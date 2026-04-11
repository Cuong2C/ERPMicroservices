namespace ItemCatalog.Api.Categories.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<GetCategoryByIdResult>;
public record GetCategoryByIdResult(Category Category);

internal class GetCategoryByIdHandler(ItemCatalogDbContext context) : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdResult>
{
    public async Task<GetCategoryByIdResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);

        if (category is null)
            throw new NotFoundException("Category", request.Id);

        return new GetCategoryByIdResult(category);
    }
}
