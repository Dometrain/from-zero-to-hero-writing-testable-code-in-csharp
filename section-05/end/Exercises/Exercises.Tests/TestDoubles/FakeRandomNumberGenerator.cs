namespace Exercises.Tests.TestDoubles;

public class FakeRandomNumberGenerator : IRandomNumberGenerator
{
    public int ValueToReturn { get; set; } = 123;

    public int Next(int minValue, int maxValue)
    {
        return ValueToReturn;
    }
}