using OrderManagement.InjectableVsNewableObjects;

namespace OrderManagement.Tests.InjectableVsNewableObjects;

public class OrderShipmentProcessorTests
{
    private readonly SpyShippingRateCalculator _shippingRateCalculator;
    private readonly OrderShipmentProcessor _processor;

    public OrderShipmentProcessorTests()
    {
        _shippingRateCalculator = new SpyShippingRateCalculator();
        _processor = new OrderShipmentProcessor(_shippingRateCalculator);
    }

    [Fact]
    public void ProcessShipment_ShouldCalculateShippingRate()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            ShippingAddress = new Address(),
            TotalWeight = 15.5m
        };

        // Act
        _processor.ProcessShipment(order);

        // Assert
        Assert.Equal(1, _shippingRateCalculator.NumberOfCalls);
    }
}

public class SpyShippingRateCalculator : IShippingRateCalculator
{
    public decimal CalculateRate(Address shippingAddress, decimal totalWeight)
    {
        NumberOfCalls++;
        return 0;
    }

    public int NumberOfCalls { get; private set; }
}