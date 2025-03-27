using Exercises.Tests.TestDoubles;

namespace Exercises.Tests;

public class OrderServiceTests
{
    [Fact]
    public void CancelOrder_WithManagerRole_ShouldAllowCancellation()
    {
        // Arrange
        var service = CreateOrderService(userRole: "Manager");

        // Act
        var result = service.CancelOrder(123, "Customer request");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CancelOrder_WithRegularUserRole_ShouldDenyPermission()
    {
        // Arrange
        var service = CreateOrderService(userRole: "User");

        // Act
        var result = service.CancelOrder(123, "Customer request");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void PlaceOrder_ShouldSetCreatedTime()
    {
        // Arrange
        var fixedTime = new DateTime(2023, 1, 1, 14, 30, 0);
        var service = CreateOrderService(fixedTime: fixedTime);

        var order = new Order
        {
            Id = 123,
            CustomerName = "Test Customer",
            Items = [new OrderItem { ProductId = 101, ProductName = "Test Product", Price = 10.00m, Quantity = 1 }]
        };

        // Act
        service.PlaceOrder(order);

        // Assert
        Assert.Equal(fixedTime, order.CreatedAt);
        Assert.Equal(OrderStatus.Completed, order.Status);
    }


    private static OrderService CreateOrderService(string userRole = "Manager", DateTime fixedTime = default)
    {
        var inventory = new FakeInventorySystem();
        var promotions = new FakePromotionManager();
        var logger = new FakeAuditLogger();
        var dateTime = new FakeDateTimeProvider(fixedTime);
        var userContext = new FakeUserContext("user", userRole);

        var processor = new OrderProcessor(
            inventory, promotions, logger, dateTime, userContext);

        var service = new OrderService(processor, dateTime, userContext);
        return service;
    }
}