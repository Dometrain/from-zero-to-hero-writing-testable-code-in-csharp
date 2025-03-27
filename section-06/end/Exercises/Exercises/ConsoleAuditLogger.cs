namespace Exercises;

public class ConsoleAuditLogger : IAuditLogger
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public ConsoleAuditLogger(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public void LogOrderProcessed(Order order, string username)
    {
        Console.WriteLine($"[AUDIT] Order {order.Id} processed by {username} at {_dateTimeProvider.GetCurrentTime()}");
    }
}