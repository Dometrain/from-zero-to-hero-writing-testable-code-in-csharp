namespace OrderManagement.StaticDependencies;

public static class Logger
{
    public static void LogError(string message)
    {
        // In a real scenario, this might write to a file or database
        Console.WriteLine($"ERROR: {message}");
    }
}