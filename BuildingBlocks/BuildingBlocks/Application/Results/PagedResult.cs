namespace BuildingBlocks.Application.Results;

internal class PagedResult<T> where T : class
{
    public IEnumerable<T>? Data { get; set; }
    public Pagination? Pagination { get; set; }
}
