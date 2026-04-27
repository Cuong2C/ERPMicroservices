namespace AuthService.Api.Apis.Users.GetUsers;

public record GetUsersQuery(int? PageNumber = 1, int? PageSize = 10, string? Keyword = null) : IRequest<GetUsersResult>;
public record GetUsersResult(PagedResult<UserDto> PagedResult);


internal class GetUsersHandler(AuthServiceDbContext context) : IRequestHandler<GetUsersQuery, GetUsersResult>
{
    public async Task<GetUsersResult> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var pageNumber = query.PageNumber ?? 1;
        var pageSize = Math.Min(query.PageSize ?? 10, 100);

        var dbQuery = context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(query.Keyword))
        {
            dbQuery = dbQuery.Where(u => u.Username.Contains(query.Keyword));
        }

        var total = await dbQuery.LongCountAsync(cancellationToken);

        var users = await dbQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto(u.Id, u.Username, u.Status, u.CreatedAt, u.CreatedBy, u.LastModifiedAt, u.LastModifiedBy))
            .ToListAsync(cancellationToken);

        var paged = new PagedResult<UserDto>
        {
            Data = users,
            Pagination = new Pagination { PageNumber = pageNumber, PageSize = pageSize, TotalCount = total }
        };

        return new GetUsersResult(paged);
    }
}
