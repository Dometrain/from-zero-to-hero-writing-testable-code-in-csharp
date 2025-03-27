using OrderManagement.FrameworkCode;

namespace OrderManagement.Tests.FrameworkCode;

public class DateTimeProviderStub : IDateTimeProvider
{
    private readonly DateTime _dateTime;

    public DateTimeProviderStub(DateTime dateTime)
    {
        _dateTime = dateTime;
    }
    public DateTime Now => _dateTime;
}

public class FakeRandomGenerator: IRandomNumberGenerator
{
    private readonly int _start;

    public FakeRandomGenerator(int start)
    {
        _start = start;
    }
    public int Next(int minValue, int maxValue)
    {
        return _start;
    }
}

public class OrderNumberGeneratorTests
{
    [Fact]
    public void GenerateOrderNumber_ReturnsCorrectFormat()
    {
        // Arrange
        const int startValue = 10;
        var date = new DateTime(2025, 1, 20);
        var generator = new OrderNumberGenerator(new DateTimeProviderStub(date), new FakeRandomGenerator(startValue));
        
        // Act
        var orderNumber = generator.GenerateOrderNumber();
        
        // Assert
        Assert.Equal("ORD-20250120000000-10", orderNumber);
    }
    
}