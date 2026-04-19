namespace ItemCatalog.Api.Apis.Tags.UpdateTag;

public record UpdateTagCommand(Guid Id, string Name) : IRequest<UpdateTagResult>;
public record UpdateTagResult(Guid Id);

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

internal class UpdateTagHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<UpdateTagCommand, UpdateTagResult>
{
    public async Task<UpdateTagResult> Handle(UpdateTagCommand command, CancellationToken cancellationToken)
    {
        var tag = await context.Tags.FindAsync(new object[] { command.Id }, cancellationToken);
        if (tag == null) throw new NotFoundException("Tag not found.");

        tenantGuard.EnsureCanAccess(tag.TenantId);

        tag.Name = command.Name;

        context.Tags.Update(tag);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateTagResult(tag.Id);
    }
}
