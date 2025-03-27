namespace Exercises.Tests.TestDoubles;

public class FakeAuditLogger : IAuditLogger
{
    private List<string> _logEntries = [];
        
    public void LogOrderProcessed(Order order, string username)
    {
        _logEntries.Add($"Order {order.Id} processed by {username}");
    }
        
    public List<string> GetLogEntries() => _logEntries;
}