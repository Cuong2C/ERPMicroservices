namespace BuildingBlocks.Application.Results;

public class CursorResult<T> where T : class
{
    public IEnumerable<T>? Data { get; set; }
    public string? NextCursor { get; set; }
    public string? PreviousCursor { get; set; }
    public bool HasNext => !string.IsNullOrEmpty(NextCursor);
    public bool HasPrevious => !string.IsNullOrEmpty(PreviousCursor);
    public string? Message { get; set; }
}
