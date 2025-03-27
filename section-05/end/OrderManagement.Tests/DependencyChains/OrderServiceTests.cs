using OrderManagement.DependencyChains;

namespace OrderManagement.Tests.DependencyChains;

internal class TenPercentDiscountProvider : IDiscountProvider
{
    public decimal GetDiscountRate()
    {
        return 0.10M;
    }
} 

public class OrderServiceTests
{
    [Fact]
    public void CalculateOrderDiscount_ReturnsCorrectDiscount()
    {
        // Arrange
        var order = new Order(1, new Customer(new LoyaltyProgram(10)), 100);
        var service = new OrderService();
        
        // Act
        var discount = service.CalculateOrderDiscount(order, new TenPercentDiscountProvider());
        
        // Assert
        Assert.Equal(10M, discount);
    }
}