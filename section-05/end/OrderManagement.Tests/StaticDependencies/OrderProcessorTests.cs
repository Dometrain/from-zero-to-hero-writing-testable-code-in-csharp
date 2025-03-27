using OrderManagement.StaticDependencies;

namespace OrderManagement.Tests.StaticDependencies;

internal class InMemoryLogger : ILogger
{
    public List<string> Messages { get; } = new();

    public void LogError(string message)
    {
        Messages.Add(message);
    }
}

public class OrderProcessorTests
{
    [Fact]
    public void ProcessOrder_ClosedOrder_ReturnsFalse()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            TotalAmount = 100.00m,
            Status = OrderStatus.Closed
        };
        var logger = new InMemoryLogger();
        var processor = new OrderProcessor(logger);

        // Act
        var result = processor.ProcessOrder(order);

        // Assert
        Assert.False(result);
        Assert.Contains($"Cannot process closed order: {order.Id}", logger.Messages);
    }

    [Fact]
    public void ProcessOrder_ClosedOrder_LogsError()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            TotalAmount = 100.00m,
            Status = OrderStatus.Closed
        };
        var logger = new InMemoryLogger();
        var processor = new OrderProcessor(logger);

        // Act
        processor.ProcessOrder(order);

        // Assert
        Assert.Contains($"Cannot process closed order: {order.Id}", logger.Messages);
    }
}