using OrderManagement.ReplacingSingletons;

namespace OrderManagement.Tests.ReplacingSingletons;

public class OrderApprovalServiceTests
{
    [Fact]
    public void CanUserApproveOrder_WithUserContextSingleton_IsHardToTest()
    {
        // Arrange
        var service = new OrderApprovalService();
        
        // We need to remember the original values to restore them later
        var originalUsername = UserContext.Current.Username;
        var originalRole = UserContext.Current.Role;
        
        try
        {
            // We have to modify the global singleton to set up our test scenario
            // This approach is extremely brittle and makes parallel testing impossible
            UserContext.Current.Username = "test.user";
            UserContext.Current.Role = "User";
            
            // Act
            var canApprove = service.CanUserApproveOrder(123, 1500);
            
            // Assert
            Assert.False(canApprove); // Regular user shouldn't approve orders > $1000
        }
        finally
        {
            // We must remember to restore the original state
            // If we forget or an exception occurs, other tests might fail
            UserContext.Current.Username = originalUsername;
            UserContext.Current.Role = originalRole;
        }
    }
    
    [Fact]
    public void CanUserApproveOrder_AsManager_ShouldAllowHighValueOrders()
    {
        // Arrange
        var service = new OrderApprovalService();
        
        // Again, we must save and modify global state
        var originalUsername = UserContext.Current.Username;
        var originalRole = UserContext.Current.Role;
        
        try
        {
            UserContext.Current.Username = "manager.user";
            UserContext.Current.Role = "Manager";
            
            // Act
            var canApprove = service.CanUserApproveOrder(123, 1500);
            
            // Assert
            Assert.True(canApprove); // Manager should approve orders > $1000
        }
        finally
        {
            // Cleanup again
            UserContext.Current.Username = originalUsername;
            UserContext.Current.Role = originalRole;
        }
    }
    
    // This test demonstrates that even with careful state management,
    // the singleton approach prevents proper test isolation
    [Fact]
    public void MultipleTests_RunningInParallel_WillInterfereWithEachOther()
    {
        // If this test runs at the same time as the other tests,
        // they will all be modifying UserContext.Current
        // creating race conditions and non-deterministic results
        
        // We can't even properly test this scenario without creating threads
        // But here's a simplified demonstration
        
        // Set manager role
        UserContext.Current.Role = "Manager";
        
        // Another test could change this value right here before our Assert
        
        // This assertion could fail if another test changed the role
        Assert.Equal("Manager", UserContext.Current.Role);
    }
}