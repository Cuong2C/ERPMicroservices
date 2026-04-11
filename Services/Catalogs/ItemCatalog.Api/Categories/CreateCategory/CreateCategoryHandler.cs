namespace ItemCatalog.Api.Categories.CreateCategory;

public record CreateCategoryCommand(string Code, string Name) : IRequest<CreateCategoryResult>;
public record CreateCategoryResult(Guid Id);

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

internal class CreateCategoryHandler(ItemCatalogDbContext context) : IRequestHandler<CreateCategoryCommand, CreateCategoryResult>
{
    public async Task<CreateCategoryResult> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = new Category { Code = command.Code, Name = command.Name };
        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return new CreateCategoryResult(category.Id);
    }
}
