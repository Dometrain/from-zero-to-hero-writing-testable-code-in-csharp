using Exercises.Tests.TestDourbles;

namespace Exercises.Tests;

public class OrderServiceTests
{


    [Fact]
    public void ProcessOrder_WithValidOrderAndPayment_ReturnsSuccessResult()
    {
        // Arrange
        var orderService = CreateOrderService();
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new() { ProductId = 1, Quantity = 1 }
            },
            Customer = new Customer()
        };
        var paymentInfo = new PaymentInfo();

        // Act
        var result = orderService.ProcessOrder(order, paymentInfo);

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Error);
    }

    [Fact]
    public void ProcessOrder_WithInsufficientInventory_ReturnsFailedResult()
    {
        // Arrange
        var orderService = CreateOrderService(hasInventory: false);
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new() { ProductId = 1, Quantity = 1 }
            },
            Customer = new Customer()
        };
        var paymentInfo = new PaymentInfo();

        // Act
        var result = orderService.ProcessOrder(order, paymentInfo);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Insufficient inventory", result.Error);
    }

    [Fact]
    public void ProcessOrder_WithFailedPayment_ReturnsFailedResult()
    {
        // Arrange
        var orderService = CreateOrderService(paymentSucceeds: false);
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new() { ProductId = 1, Quantity = 1 }
            },
            Customer = new Customer()
        };
        var paymentInfo = new PaymentInfo();

        // Act
        var result = orderService.ProcessOrder(order, paymentInfo);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Payment failed", result.Error);
    }

    [Fact]
    public void ProcessOrder_WhenSuccessful_UpdatesOrderProperties()
    {
        // Arrange
        var orderService = CreateOrderService(orderTotal: 150m);
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new() { ProductId = 1, Quantity = 1 }
            },
            Customer = new Customer()
        };
        var paymentInfo = new PaymentInfo();

        // Act
        orderService.ProcessOrder(order, paymentInfo);

        // Assert
        Assert.Equal(150m, order.TotalAmount);
        Assert.Equal("Completed", order.Status);
        Assert.Equal(new DateTime(2024, 3, 18), order.OrderDate);
    }
    
    private OrderService CreateOrderService(
        bool hasInventory = true,
        decimal orderTotal = 100m,
        bool paymentSucceeds = true)
    {
        return new OrderService(
            new InventoryServiceStub(hasInventory),
            new PricingServiceStub(orderTotal),
            new PaymentServiceStub(paymentSucceeds),
            new CustomerServiceSpy(),
            new NotificationServiceSpy(),
            new OrderRepositorySpy(),
            new DateTimeProviderStub(),
            new LoggerSpy());
    }

}