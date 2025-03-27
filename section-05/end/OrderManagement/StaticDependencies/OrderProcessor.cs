namespace OrderManagement.StaticDependencies;

public class OrderProcessor
{
    private readonly ILogger _logger;

    public OrderProcessor(ILogger logger)
    {
        _logger = logger;
    }
    public bool ProcessOrder(Order order)
    {
        if (order.Status == OrderStatus.Closed)
        {
            _logger.LogError($"Cannot process closed order: {order.Id}");
            return false;
        }
        
        // Processing logic would go here...
        return true;
    }
}
