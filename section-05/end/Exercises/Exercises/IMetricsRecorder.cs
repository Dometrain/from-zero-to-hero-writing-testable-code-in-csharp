namespace Exercises;

public interface IMetricsRecorder
{
    void RecordOrderProcessed(Order order);
    void RecordOrderCancelled(Order order, string reason);
    void IncrementCounter(string counterName);
}