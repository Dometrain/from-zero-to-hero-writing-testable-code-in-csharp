namespace OrderManagement.StaticDependencies;

public interface ILogger
{
    public void LogError(string message);
}
public class Logger : ILogger
{
    public void LogError(string message)
    {
        // In a real scenario, this might write to a file or database
        Console.WriteLine($"ERROR: {message}");
    }
}