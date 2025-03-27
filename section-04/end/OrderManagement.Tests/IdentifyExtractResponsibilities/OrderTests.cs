using OrderManagement.IdentifyExtractResponsibilities;

namespace OrderManagement.Tests.IdentifyExtractResponsibilities;

public class OrderTests
{
    [Fact]
    public void TotalAmount_WithMultipleItems_ShouldCalculateCorrectly()
    {
        // Arrange
        var order = new Order
        {
            OrderId = 1,
            Customer = new Customer(),
            Items = new List<OrderItem>
            {
                new() { Price = 10.0m, Quantity = 2 },
                new() { Price = 15.0m, Quantity = 3 }
            }
        };

        // Act
        var totalAmount = order.TotalAmount;

        // Assert
        Assert.Equal(65.0m, totalAmount); // (10 * 2) + (15 * 3) = 20 + 45 = 65
    }

    [Fact]
    public void TotalAmount_WithEmptyItems_ShouldBeZero()
    {
        // Arrange
        var order = new Order
        {
            OrderId = 1,
            Customer = new Customer(),
            Items = new List<OrderItem>()
        };

        // Act
        var totalAmount = order.TotalAmount;

        // Assert
        Assert.Equal(0m, totalAmount);
    }
}