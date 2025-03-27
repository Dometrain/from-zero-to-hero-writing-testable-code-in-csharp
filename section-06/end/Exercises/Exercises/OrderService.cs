namespace Exercises;

public class OrderService
{
    private readonly OrderProcessor _orderProcessor;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserContext _userContext;

    public OrderService(
        OrderProcessor orderProcessor,
        IDateTimeProvider dateTimeProvider,
        IUserContext userContext)
    {
        _orderProcessor = orderProcessor;
        _dateTimeProvider = dateTimeProvider;
        _userContext = userContext;
    }

    public bool PlaceOrder(Order order)
    {
        order.CreatedAt = _dateTimeProvider.GetCurrentTime();

        order.Status = OrderStatus.Pending;

        var success = _orderProcessor.ProcessOrder(order);

        if (success)
        {
            Console.WriteLine($"Order {order.Id} for {order.CustomerName} processed successfully");
        }
        else
        {
            Console.WriteLine($"Failed to process order {order.Id}");
        }

        return success;
    }

    public bool CancelOrder(int orderId, string reason)
    {
        if (_userContext.GetRole() != "Manager" && _userContext.GetRole() != "Admin")
        {
            Console.WriteLine($"User {_userContext.GetUsername()} not authorized to cancel orders");
            return false;
        }

        // Find order (simplified, would come from database)
        var order = new Order { Id = orderId, Status = OrderStatus.Pending };

        order.Status = OrderStatus.Cancelled;

        Console.WriteLine($"Order {order.Id} cancelled by {_userContext.GetUsername()}: {reason}");
        return true;
    }
}