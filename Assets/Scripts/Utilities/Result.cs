public class Result
{
    public bool Success { get; }
    public string Error { get; }

    private Result(bool success, string error = null)
    {
        Success = success;
        Error = error;
    }

    public static Result Ok() => new Result(true);
    public static Result Fail(string error) => new Result(false, error);
}