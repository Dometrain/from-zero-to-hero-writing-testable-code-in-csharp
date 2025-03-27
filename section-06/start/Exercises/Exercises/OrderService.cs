namespace Exercises;

public class OrderService
{
    public bool PlaceOrder(Order order)
    {
        // Set created timestamp
        order.CreatedAt = DateTime.Now;

        // Set initial status
        order.Status = OrderStatus.Pending;

        // Process the order
        var processor = new OrderProcessor();
        var success = processor.ProcessOrder(order);

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
        if (UserContext.Current.Role != "Manager" && UserContext.Current.Role != "Admin")
        {
            Console.WriteLine($"User {UserContext.Current.Username} not authorized to cancel orders");
            return false;
        }

        // Find order (simplified, would come from database)
        var order = new Order { Id = orderId, Status = OrderStatus.Pending };

        // Cancel order
        order.Status = OrderStatus.Cancelled;

        Console.WriteLine($"Order {order.Id} cancelled by {UserContext.Current.Username}: {reason}");
        return true;
    }
}