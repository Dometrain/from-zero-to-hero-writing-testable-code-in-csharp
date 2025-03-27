namespace Exercises;

public class OrderRepository : IOrderRepository
{
    private readonly List<Order> _orders;
    private readonly ILogger _logger;

    public OrderRepository(List<Order> orders, ILogger logger)
    {
        _orders = orders;
        _logger = logger;
    }

    public void SaveOrder(Order order)
    {
        if (!_orders.Contains(order))
        {
            _orders.Add(order);
        }
        _logger.LogInformation($"Order {order.Id} saved");
    }

    public Order GetOrder(int id)
    {
        return _orders.FirstOrDefault(o => o.Id == id);
    }
}