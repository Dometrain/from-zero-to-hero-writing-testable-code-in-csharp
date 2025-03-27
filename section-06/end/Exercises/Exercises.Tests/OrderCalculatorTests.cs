namespace Exercises.Tests;

public class OrderCalculatorTests
{
    private readonly OrderCalculator _calculator;

    public OrderCalculatorTests()
    {
        _calculator = new OrderCalculator();
    }

    [Theory]
    [InlineData(10.00, 2, 20.00)]
    [InlineData(25.00, 1, 25.00)]
    [InlineData(100.00, 3, 300.00)]
    public void CalculateOrderTotal_WithValidItems_ShouldSumCorrectly(
        decimal price,
        int quantity,
        decimal expectedTotal)
    {
        // Arrange
        var order = CreateTestOrder(price, quantity);

        // Act
        var total = _calculator.CalculateOrderTotal(order, 0);

        // Assert
        Assert.Equal(expectedTotal, total);
        Assert.Equal(0, order.TotalAmount); // Verify no side effects
    }

    [Theory]
    [InlineData(100.00, 10, 90.00)]
    [InlineData(200.00, 20, 160.00)]
    [InlineData(50.00, 5, 47.50)]
    public void CalculateOrderTotal_WithDiscount_ShouldApplyCorrectly(
        decimal price,
        decimal discountPercentage,
        decimal expectedTotal)
    {
        // Arrange
        var order = CreateTestOrder(price);

        // Act
        var total = _calculator.CalculateOrderTotal(order, discountPercentage);

        // Assert
        Assert.Equal(expectedTotal, total);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(50)]
    public void CalculateOrderTotal_WithZeroItems_ShouldReturnZero(decimal discountPercentage)
    {
        // Arrange
        var order = CreateTestOrder(quantity: 0);

        // Act
        var total = _calculator.CalculateOrderTotal(order, discountPercentage);

        // Assert
        Assert.Equal(0, total);
    }

    [Fact]
    public void CalculateOrderTotal_MultipleTimesWithSameInput_ShouldProduceSameResult()
    {
        // Arrange
        var order = CreateTestOrder(10.00m, 2);

        // Act
        var firstResult = _calculator.CalculateOrderTotal(order, 0);
        var secondResult = _calculator.CalculateOrderTotal(order, 0);

        // Assert
        Assert.Equal(firstResult, secondResult);
    }


    private static Order CreateTestOrder(decimal price = 10.00m, int quantity = 1)
    {
        return new Order
        {
            Items = [new OrderItem { ProductId = 101, Price = price, Quantity = quantity }]
        };
    }
}