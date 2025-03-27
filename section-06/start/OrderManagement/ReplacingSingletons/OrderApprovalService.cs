namespace OrderManagement.ReplacingSingletons;

public class OrderApprovalService
{
    public bool CanUserApproveOrder(int orderId, decimal orderAmount)
    {
        var currentUser = UserContext.Current.Username;
        var userRole = UserContext.Current.Role;
        
        // Business rules for order approval
        if (orderAmount > 1000 && userRole != "Manager")
        {
            return false;
        }
        
        return true;
    }
}