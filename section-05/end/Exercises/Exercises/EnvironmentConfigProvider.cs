namespace Exercises;

public class EnvironmentConfigProvider : IConfigProvider
{
    public string GetOrderPrefix()
    {
        var environment = Environment.GetEnvironmentVariable("APP_ENV") ?? "dev";

        return environment switch
        {
            "production" => "PROD",
            "staging" => "STG",
            "test" => "TEST",
            _ => "DEV"
        };
    }

    public int GetOrderStartSequence()
    {
        var startSeq = Environment.GetEnvironmentVariable("ORDER_START_SEQ");

        if (int.TryParse(startSeq, out var result))
        {
            return result;
        }

        return 10000; // Default start sequence
    }
}