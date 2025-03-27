using OrderManagement.AvoidingTestInterference;

namespace OrderManagement.Tests.AvoidingTestInterference;

public class DiscountManagerTests
{
    [Fact]
    public void ApplyRegularCustomerDiscount_ShouldApplyCorrectPercentage()
    {
        // Arrange
        try
        {
            // We have to manually set up the global state
            DiscountManager.ActiveDiscountPercentage = 10;

            var order = new Order
            {
                Id = 101,
                CustomerId = 1001,
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductId = 5, Price = 100, Quantity = 1 }
                }
            };

            var calculator = new OrderCalculator();

            // Act
            var total = calculator.CalculateTotal(order);

            // Assert
            Assert.Equal(90, total); // Expects 10% discount
        }
        finally
        {
            // We have to remember to clean up global state to prevent test interference
            // If this cleanup is forgotten or fails to execute, other tests will break
            DiscountManager.ActiveDiscountPercentage = 0;
        }
    }

    [Fact]
    public void ApplyPremiumCustomerDiscount_ShouldApplyCorrectPercentage()
    {
        // Arrange
        try
        {
            // We have to manually set up the global state again
            // We have to remember what the default value should be
            DiscountManager.ActiveDiscountPercentage = 0;

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

            var calculator = new OrderCalculator();

            // Act
            var total = calculator.CalculateTotal(order);

            // Assert
            Assert.Equal(85, total); // Expects 15% discount for premium customers
        }
        finally
        {
            // Cleanup again
            DiscountManager.ActiveDiscountPercentage = 0;
        }
    }

    // This test demonstrates that even with try/finally cleanup,
    // tests can still interfere when run in parallel
    [Fact]
    public void ParallelExecutionDemonstration()
    {
        // If this test runs at the same time as the first test,
        // one will set ActiveDiscountPercentage to 10% and the other to 0%
        // Leading to race conditions and non-deterministic test results

        DiscountManager.ActiveDiscountPercentage = 0;

        // Even a small delay here could allow another test to change the value
        Thread.Sleep(10);

        // This assertion might fail if another test changed the value
        Assert.Equal(0, DiscountManager.ActiveDiscountPercentage);
    }
}