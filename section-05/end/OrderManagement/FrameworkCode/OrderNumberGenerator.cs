namespace OrderManagement.FrameworkCode;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}
public interface IRandomNumberGenerator
{
    int Next(int minValue, int maxValue);
}
public class SystemRandomGenerator : IRandomNumberGenerator
{
    private readonly Random _random = new Random();
    
    public int Next(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }
}



public class OrderNumberGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRandomNumberGenerator _randomNumberGenerator;

    public OrderNumberGenerator(IDateTimeProvider dateTimeProvider, IRandomNumberGenerator randomNumberGenerator)
    {
        _dateTimeProvider = dateTimeProvider;
        _randomNumberGenerator = randomNumberGenerator;
    }

    public OrderNumberGenerator()
    {
        _dateTimeProvider = new SystemDateTimeProvider();
        _randomNumberGenerator = new SystemRandomGenerator();
    }
    
    public string GenerateOrderNumber()
    {
        // Generate a unique order number using current timestamp and random numbers
        var timestamp = _dateTimeProvider.Now.ToString("yyyyMMddHHmmss");
        var randomPart = _randomNumberGenerator.Next(1000, 9999).ToString();
        
        return $"ORD-{timestamp}-{randomPart}";
    }
}