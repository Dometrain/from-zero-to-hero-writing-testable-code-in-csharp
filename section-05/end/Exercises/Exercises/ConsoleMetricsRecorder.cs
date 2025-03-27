namespace Exercises;

public class ConsoleMetricsRecorder : IMetricsRecorder
{
    public void RecordOrderProcessed(Order order)
    {
        Console.WriteLine($"METRIC: Order {order.Id} processed at {DateTime.Now}");
    }

    public void RecordOrderCancelled(Order order, string reason)
    {
        Console.WriteLine($"METRIC: Order {order.Id} cancelled. Reason: {reason}");
    }

    public void IncrementCounter(string counterName)
    {
        Console.WriteLine($"METRIC: Incremented counter {counterName}");
    }
}