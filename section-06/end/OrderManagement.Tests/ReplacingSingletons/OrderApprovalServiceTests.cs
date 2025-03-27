using OrderManagement.ReplacingSingletons;

namespace OrderManagement.Tests.ReplacingSingletons;

public class OrderUserApprovalServiceTests
{
    [Fact]
    public void CanUserApproveOrder_WithUserContextSingleton_IsHardToTest()
    {
        // Arrange
        var userContext = new UserContext();
        userContext.SetUser("test.user", "User");
        var service = new OrderApprovalService(userContext);

        // Act
        var canApprove = service.CanUserApproveOrder(123, 1500);

        // Assert
        Assert.False(canApprove); // Regular user shouldn't approve orders > $1000
    }
}

public class OrderManagerApprovalServiceTests
{
    [Fact]
    public void CanUserApproveOrder_AsManager_ShouldAllowHighValueOrders()
    {
        // Arrange
        var userContext = new UserContext();
        userContext.SetUser("manager.user", "Manager");
        var service = new OrderApprovalService(userContext);

        // Act
        var canApprove = service.CanUserApproveOrder(123, 1500);

        // Assert
        Assert.True(canApprove); // Manager should approve orders > $1000
    }
}