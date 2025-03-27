using Exercises.Tests.TestDoubles;

namespace Exercises.Tests;

public class OrderProcessorTests
{
    private readonly OrderProcessor _processor;
    private readonly FakeInventorySystem _inventory;
    private readonly FakePromotionManager _promotions;
    private readonly FakeAuditLogger _logger;
    private readonly FakeDateTimeProvider _dateTime;

    public OrderProcessorTests()
    {
        _inventory = new FakeInventorySystem();
        _promotions = new FakePromotionManager();
        _logger = new FakeAuditLogger();
        _dateTime = new FakeDateTimeProvider(new DateTime(2023, 1, 1, 12, 0, 0));
        var userContext = new FakeUserContext("test.user", "User");
        _processor = new OrderProcessor(_inventory, _promotions, _logger, _dateTime, userContext);
    }

    [Fact]
    public void ProcessOrder_WithAvailableInventory_ShouldCompleteOrder()
    {
        // Arrange
        var order = CreateTestOrder(quantity: 2);

        // Act
        var result = _processor.ProcessOrder(order);

        // Assert
        Assert.True(result, "Order processing should succeed with available inventory");
        Assert.Equal(OrderStatus.Completed, order.Status);
        Assert.Equal(20.00m, order.TotalAmount);
        Assert.Equal(_dateTime.GetCurrentTime(), order.ProcessedAt);

        // Verify inventory operations
        var operations = _inventory.GetOperations();
        Assert.Contains("CheckStock: 101, 2", operations);
        Assert.Contains("DeductStock: 101, 2", operations);
    }

    [Fact]
    public void ProcessOrder_WithUnavailableInventory_ShouldCancelOrder()
    {
        // Arrange
        var order = CreateTestOrder(quantity: 200);

        // Act
        var result = _processor.ProcessOrder(order);

        // Assert
        Assert.False(result, "Order processing should fail with insufficient inventory");
        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Fact]
    [Trait("Category", "Promotions")]
    public void ProcessOrder_WithActivePromotion_ShouldApplyDiscount()
    {
        // Arrange
        _promotions.StartPromotion(10); // 10% discount
        var order = CreateTestOrder(price: 100.00m, quantity: 1);

        // Act
        _processor.ProcessOrder(order);

        // Assert
        Assert.Equal(90.00m, order.TotalAmount); // 10% off 100
    }

    [Fact]
    public void ProcessOrder_ShouldLogAllOperations()
    {
        // Arrange
        var order = CreateTestOrder();

        // Act
        _processor.ProcessOrder(order);

        // Assert
        var logEntries = _logger.GetLogEntries();
        Assert.Contains(logEntries, entry => entry.Contains($"Order {order.Id} processed by"));
    }

    private static Order CreateTestOrder(
        int productId = 101,
        string productName = "Test Product",
        decimal price = 10.00m,
        int quantity = 1)
    {
        return new Order
        {
            Id = 123,
            CustomerName = "Test Customer",
            Status = OrderStatus.Pending,
            Items = [new OrderItem { ProductId = productId, ProductName = productName, Price = price, Quantity = quantity }]
        };
    }
}