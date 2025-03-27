namespace Exercises.Tests.TestDoubles;

public class FakeConfigProvider : IConfigProvider
{
    public string PrefixToReturn { get; set; } = "TEST";
    public int SequenceToReturn { get; set; } = 1000;

    public string GetOrderPrefix()
    {
        return PrefixToReturn;
    }

    public int GetOrderStartSequence()
    {
        return SequenceToReturn;
    }
}