namespace OrderManagement.ReplacingSingletons;

public class OrderApprovalService
{
    private readonly IUserContext _userContext;

    public OrderApprovalService(IUserContext userContext)
    {
        _userContext = userContext;
    }

    public bool CanUserApproveOrder(int orderId, decimal orderAmount)
    {
        var currentUser = _userContext.GetUsername();
        var userRole = _userContext.GetRole();
        
        // Business rules for order approval
        if (orderAmount > 1000 && userRole != "Manager")
        {
            return false;
        }
        
        return true;
    }
}