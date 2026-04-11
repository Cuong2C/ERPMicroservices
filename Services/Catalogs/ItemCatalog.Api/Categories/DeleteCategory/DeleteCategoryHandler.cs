namespace ItemCatalog.Api.Categories.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<DeleteCategoryResult>;
public record DeleteCategoryResult(Guid Id);

internal class DeleteCategoryHandler(ItemCatalogDbContext context) : IRequestHandler<DeleteCategoryCommand, DeleteCategoryResult>
{
    public async Task<DeleteCategoryResult> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync(new object[] { command.Id }, cancellationToken);
        if (category == null) throw new NotFoundException("Category not found.");

        var used = await context.ItemCategories.AnyAsync(ic => ic.CategoryId == category.Id, cancellationToken);
        if (used) throw new BadRequestException("Category is in use by items and cannot be deleted.");

        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteCategoryResult(category.Id);
    }
}
