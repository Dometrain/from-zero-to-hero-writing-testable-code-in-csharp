namespace Exercises;

public static class Metrics
{
    public static void RecordOrderProcessed(Order order)
    {
        // In a real app, this might send data to a monitoring system
        Console.WriteLine($"METRIC: Order {order.Id} processed at {DateTime.Now}");
    }
        
    public static void RecordOrderCancelled(Order order, string reason)
    {
        Console.WriteLine($"METRIC: Order {order.Id} cancelled. Reason: {reason}");
    }
        
    public static void IncrementCounter(string counterName)
    {
        Console.WriteLine($"METRIC: Incremented counter {counterName}");
    }
}