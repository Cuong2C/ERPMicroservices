namespace ItemCatalog.Api.Tags.DeleteTag;

public record DeleteTagCommand(Guid Id) : IRequest<DeleteTagResult>;
public record DeleteTagResult(Guid Id);

internal class DeleteTagHandler(ItemCatalogDbContext context, ITenantGuard tenantGuard) : IRequestHandler<DeleteTagCommand, DeleteTagResult>
{
    public async Task<DeleteTagResult> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        var tag = await context.Tags.FindAsync(new object[] { command.Id }, cancellationToken);
        if (tag == null) throw new NotFoundException("Tag not found.");

        tenantGuard.EnsureCanAccess(tag.TenantId);

        var used = await context.Items.AnyAsync(i => i.Tags.Any(t => t.Id == tag.Id), cancellationToken);
        if (used) throw new BadRequestException("Tag is in use by items and cannot be deleted.");

        context.Tags.Remove(tag);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteTagResult(tag.Id);
    }
}
