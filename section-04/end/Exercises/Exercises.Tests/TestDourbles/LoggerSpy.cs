namespace Exercises.Tests.TestDourbles;

public class LoggerSpy : ILogger
{
    public List<string> ErrorLogs { get; } = new();
    public List<string> WarningLogs { get; } = new();

    public void LogError(string message) => ErrorLogs.Add(message);

    public void LogInformation(string message)
    {
    }

    public void LogWarning(string message) => WarningLogs.Add(message);
}