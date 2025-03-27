using OrderManagement.IdentifyExtractResponsibilities;

namespace OrderManagement.Tests.IdentifyExtractResponsibilities;

public class CustomerLoyaltyManagerTests
{
    private readonly CustomerLoyaltyManager _loyaltyManager = new();

    [Fact]
    public void UpdateLoyaltyPoints_ShouldAddCorrectPoints()
    {
        // Arrange
        var customer = new Customer { LoyaltyPoints = 0 };
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new() { Price = 100.0m, Quantity = 1 }
            }
        };

        // Act
        _loyaltyManager.UpdateLoyaltyPoints(customer, order);

        // Assert
        Assert.Equal(10, customer.LoyaltyPoints); // 100/10 = 10 points
    }

    [Fact]
    public void UpdateLoyaltyPoints_WhenPointsExceed1000_ShouldSetGoldTier()
    {
        // Arrange
        var customer = new Customer { LoyaltyPoints = 995 };
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new() { Price = 100.0m, Quantity = 1 }
            }
        };

        // Act
        _loyaltyManager.UpdateLoyaltyPoints(customer, order);

        // Assert
        Assert.Equal(LoyaltyTier.Gold, customer.LoyaltyTier);
    }

    [Fact]
    public void UpdateLoyaltyPoints_WhenPointsExceed500_ShouldSetSilverTier()
    {
        // Arrange
        var customer = new Customer { LoyaltyPoints = 495 };
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new() { Price = 100.0m, Quantity = 1 }
            }
        };

        // Act
        _loyaltyManager.UpdateLoyaltyPoints(customer, order);

        // Assert
        Assert.Equal(LoyaltyTier.Silver, customer.LoyaltyTier);
    }
}