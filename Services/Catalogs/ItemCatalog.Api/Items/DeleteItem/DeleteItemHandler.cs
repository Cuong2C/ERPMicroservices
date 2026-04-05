namespace ItemCatalog.Api.Items.DeleteItem;

public record DeleteItemCommand(Guid Id) : IRequest<DeleteItemResult>;

public record DeleteItemResult(bool IsSuccess);

internal class DeleteItemHandler(ItemCatalogDbContext context) : IRequestHandler<DeleteItemCommand, DeleteItemResult>
{
    public async Task<DeleteItemResult> Handle(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        var item = await context.Items.FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken);

        if (item == null)
            throw new NotFoundException("Item not found.");

        // soft delete
        item.Status = Status.Deleted;

        context.Items.Update(item);

        await context.SaveChangesAsync(cancellationToken);

        return new DeleteItemResult(true);
    }
}
