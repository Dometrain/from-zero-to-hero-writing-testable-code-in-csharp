using OrderManagement.InjectableVsNewableObjects;

namespace OrderManagement.Tests.InjectableVsNewableObjects;

public class ShippingRateCalculatorTests
{
    private readonly ShippingRateCalculator _calculator;

    public ShippingRateCalculatorTests()
    {
        _calculator = new ShippingRateCalculator();
    }

    [Fact]
    public void CalculateRate_ShouldReturnCorrectRate()
    {
        // Arrange
        var address = new Address(); // Address details don't affect calculation in current implementation
        const decimal weight = 10.0m;
        var expectedRate = (weight * 0.5m) + 5.99m; // Based on the formula in ShippingRateCalculator

        // Act
        var rate = _calculator.CalculateRate(address, weight);

        // Assert
        Assert.Equal(expectedRate, rate);
    }

    [Fact]
    public void CalculateRate_WithZeroWeight_ShouldReturnBaseRate()
    {
        // Arrange
        var address = new Address();
        const decimal weight = 0m;
        const decimal expectedRate = 5.99m; // Only base rate when weight is zero

        // Act
        var rate = _calculator.CalculateRate(address, weight);

        // Assert
        Assert.Equal(expectedRate, rate);
    }
} 