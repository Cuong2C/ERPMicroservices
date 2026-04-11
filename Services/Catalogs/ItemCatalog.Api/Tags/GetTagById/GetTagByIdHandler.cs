namespace ItemCatalog.Api.Tags.GetTagById;

public record GetTagByIdQuery(Guid Id) : IRequest<GetTagByIdResult>;
public record GetTagByIdResult(Tag Tag);

internal class GetTagByIdHandler(ItemCatalogDbContext context) : IRequestHandler<GetTagByIdQuery, GetTagByIdResult>
{
    public async Task<GetTagByIdResult> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await context.Tags.FindAsync(new object[] { request.Id }, cancellationToken);

        if (tag == null)
            throw new NotFoundException("Tag", request.Id);

        return new GetTagByIdResult(tag);
    }
}
