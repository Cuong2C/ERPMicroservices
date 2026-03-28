namespace BuildingBlocks.Application.Results;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public static Result<T> Success(T data)
        => new() { IsSuccess = true, Data = data };

    public static Result<T> Failure(string error)
        => new() { IsSuccess = false, Message = error };
}
