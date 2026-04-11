namespace ItemCatalog.Api.Tags.CreateTag;

public record CreateTagCommand(string Name) : IRequest<CreateTagResult>;
public record CreateTagResult(Guid Id);

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

internal class CreateTagHandler(ItemCatalogDbContext context) : IRequestHandler<CreateTagCommand, CreateTagResult>
{
    public async Task<CreateTagResult> Handle(CreateTagCommand command, CancellationToken cancellationToken)
    {
        var tag = new Tag { Name = command.Name };
        await context.Tags.AddAsync(tag, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return new CreateTagResult(tag.Id);
    }
}
