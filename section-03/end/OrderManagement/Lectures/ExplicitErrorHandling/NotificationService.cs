namespace OrderManagement.Lectures.ExplicitErrorHandling;

public class NotificationService
{
    public Result<Notification> SendDeliveryUpdate(Order order, Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(order);

        if (string.IsNullOrEmpty(customer.Email))
        {
            return Result<Notification>.Failure(new Error("Email is required"));
        }

        if (!customer.Email.Contains("@"))
        {
            return Result<Notification>.Failure(new Error("Invalid email"));
        }

        // Send notification here
        
        return Result<Notification>.Success(
            new Notification(customer.Email, "Delivery update"));
    }
}

public record Notification(string Email, string Subject);

public readonly record struct Error(string Message);

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