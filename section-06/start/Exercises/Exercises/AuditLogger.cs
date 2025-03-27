namespace Exercises;

public static class AuditLogger
{
    public static void LogOrderProcessed(Order order)
    {
        var user = UserContext.Current.Username;
        Console.WriteLine($"[AUDIT] Order {order.Id} processed by {user} at {DateTime.Now}");
    }
}