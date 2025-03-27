namespace OrderManagement.Api.Core;

public static class ErrorCodes
{
    public const string InvalidOrder = "InvalidOrder";
    public const string NotFound = "NotFound";
    public const string ShippingFailure = "ShippingFailure";
}

public readonly record struct Error(string ErrorCode, string Message);

public readonly record struct Result<T>
{
    public T? Value { get; }
    public Error? Error { get; }
    public bool IsSuccess => Error is null;

    private Result(T value)
    {
        Value = value;
        Error = null;
    }

    private Result(Error error)
    {
        Value = default;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);
}