using OrderManagement.DependencyChains;

namespace OrderManagement.Tests.DependencyChains;

public class OrderServiceTests
{
    [Fact]
    public void CalculateOrderDiscount_ReturnsCorrectDiscount()
    {
        // Arrange
        var order = new Order(1, new Customer(new LoyaltyProgram(10)), 100);
        var service = new OrderService();
        
        // PROBLEM: We can't easily test this method because:
        // 1. We can't mock the Customer returned by order.GetCustomer()
        // 2. We can't mock the LoyaltyProgram returned by customer.GetLoyaltyProgram()
        // 3. We can't mock the discount rate returned by loyaltyProgram.GetDiscountRate()
        
        // If we tried to use a mocking framework to mock these dependencies
        // it won't work because these classes don't have interfaces
        // and the methods aren't virtual
        
        // Act & Assert
        // We can't even properly set up this test without creating
        // real instances of all dependent objects with specific values
        
        // PROBLEM: To test different discount scenarios, we'd need to
        // create and configure entire object graphs
    }
    
}