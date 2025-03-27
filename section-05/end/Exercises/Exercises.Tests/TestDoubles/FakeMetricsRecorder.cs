namespace Exercises.Tests.TestDoubles;

public class FakeMetricsRecorder : IMetricsRecorder
{
    public List<string> RecordedMetrics { get; } = new List<string>();

    public void RecordOrderProcessed(Order order)
    {
        RecordedMetrics.Add($"OrderProcessed:{order.Id}");
    }

    public void RecordOrderCancelled(Order order, string reason)
    {
        RecordedMetrics.Add($"OrderCancelled:{order.Id}:{reason}");
    }

    public void IncrementCounter(string counterName)
    {
        RecordedMetrics.Add($"Counter:{counterName}");
    }
}