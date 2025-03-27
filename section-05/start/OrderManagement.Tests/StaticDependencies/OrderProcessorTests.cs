using OrderManagement.StaticDependencies;

namespace OrderManagement.Tests.StaticDependencies;

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
        var processor = new OrderProcessor();

        // Act
        var result = processor.ProcessOrder(order);

        // Assert
        Assert.False(result);

        // PROBLEM: We cannot verify that Logger.LogError was called
        // with the correct message. The static Logger dependency 
        // makes it impossible to mock or verify the logging behavior.

        // PROBLEM: We can't easily test different scenarios with the logger:
        // - What if the logger throws an exception?
        // - What if we want to verify the exact error message?
        // - What if we're running tests in parallel and logs get mixed?
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
        var processor = new OrderProcessor();

        // We need to capture console output to verify logging
        var originalOutput = Console.Out;
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        try
        {
            // Act
            processor.ProcessOrder(order);

            // Assert
            var output = stringWriter.ToString();
            Assert.Contains($"ERROR: Cannot process closed order: {order.Id}", output);

            // PROBLEM: This test is fragile because:
            // 1. It depends on Console.Out which affects other tests
            // 2. It knows implementation details of Logger
            // 3. If Logger implementation changes, test breaks
            // 4. We are testing two things: OrderProcessor AND Logger
        }
        finally
        {
            // Clean up
            Console.SetOut(originalOutput);
        }
    }
}