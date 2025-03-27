using OrderManagement.AvoidingTestInterference;

namespace OrderManagement.Tests.AvoidingTestInterference;

public class DiscountManagerTests
{
    private readonly DiscountManager _discountManager;
    private readonly OrderCalculator _calculator;

    public DiscountManagerTests()
    {
         _discountManager = new DiscountManager();
         _calculator = new OrderCalculator(_discountManager);
    }
    
    [Fact]
    public void ApplyRegularCustomerDiscount_ShouldApplyCorrectPercentage()
    {
        // Arrange
        _discountManager.SetActiveDiscountPercentage(10);
        var order = new Order
        {
            Id = 101,
            CustomerId = 1001,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 5, Price = 100, Quantity = 1 }
            }
        };
        
        // Act
        var total = _calculator.CalculateTotal(order);

        // Assert
        Assert.Equal(90, total); // Expects 10% discount
    }

    [Fact]
    public void ApplyPremiumCustomerDiscount_ShouldApplyCorrectPercentage()
    {
        // Arrange
        var order = new Order
        {
            Id = 102,
            CustomerId = 2002,
            CustomerType = CustomerType.Premium,
            Items = new List<OrderItem>
            {
                new OrderItem { ProductId = 10, Price = 100, Quantity = 1 }
            }
        };


        // Act
        var total = _calculator.CalculateTotal(order);

        // Assert
        Assert.Equal(85, total); // Expects 15% discount for premium customers
    }
}