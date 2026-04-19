namespace ItemCatalog.Api.Apis.Categories.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Code, string Name) : IRequest<UpdateCategoryResult>;
public record UpdateCategoryResult(Guid Id);

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

internal class UpdateCategoryHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResult>
{
    public async Task<UpdateCategoryResult> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync(new object[] { command.Id }, cancellationToken);
        if (category == null) throw new NotFoundException("Category not found.");

        tenantGuard.EnsureCanAccess(category.TenantId);

        category.Code = command.Code;
        category.Name = command.Name;

        context.Categories.Update(category);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateCategoryResult(category.Id);
    }
}
