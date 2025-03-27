namespace OrderManagement.StaticDependencies;

public class OrderProcessor
{
    public bool ProcessOrder(Order order)
    {
        if (order.Status == OrderStatus.Closed)
        {
            Logger.LogError($"Cannot process closed order: {order.Id}");
            return false;
        }
        
        // Processing logic would go here...
        return true;
    }
}
