namespace Exercises;

public interface IAuditLogger
{
    void LogOrderProcessed(Order order, string username);
}