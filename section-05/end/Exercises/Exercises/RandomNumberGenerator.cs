namespace Exercises;

public class RandomNumberGenerator : IRandomNumberGenerator
{
    private readonly Random _random = new Random();

    public int Next(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }
}