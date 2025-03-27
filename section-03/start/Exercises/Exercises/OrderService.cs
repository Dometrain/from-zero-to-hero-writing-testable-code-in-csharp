namespace Exercises;

public class OrderService
{
    private List<Product> _productInventory;
    private List<Order> _orders;
    private Customer _currentCustomer;
    private readonly string _storeLocation;
    private readonly bool _expressShippingAvailable;

    public OrderService(string storeLocation)
    {
        _productInventory = new List<Product>();
        _orders = new List<Order>();

        _storeLocation = storeLocation;

        _expressShippingAvailable = storeLocation == "Warehouse" || storeLocation == "Mall";

        Console.WriteLine($"Store {storeLocation} initialized with express shipping: {_expressShippingAvailable}");
    }

    public bool ProcessOrder(string orderId, int newStatus)
    {
        var order = _orders.FirstOrDefault(o => o.Id == orderId);

        if (order != null)
        {
            order.Status = newStatus;

            // Some processing logic

            return true;
        }

        return false;
    }

    public void CreateOrder(List<OrderItem> items, bool? priority = null, string notes = "NONE")
    {
        if (_currentCustomer == null)
        {
            throw new Exception("No customer selected");
        }

        var order = new Order
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = _currentCustomer.Id,
            Items = items,
            OrderDate = DateTime.Now,
            Status = 0
        };

        foreach (var item in items)
        {
            var product = _productInventory.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null)
            {
                continue;
            }

            if (product.StockQuantity < item.Quantity)
            {
                return;
            }

            product.StockQuantity -= item.Quantity;
        }

        if (priority == null)
            priority = false;

        if (priority.Value)
        {
            // Priority handling
            // ...
        }

        _orders.Add(order);
    }

    public bool SetCustomer(string customerId)
    {
        var customer = FindCustomer(customerId);
        if (customer != null)
        {
            _currentCustomer = customer;
            return true;
        }

        return false;
    }

    private Customer FindCustomer(string customerId)
    {
        // This would typically be a database lookup
        return new Customer { Id = customerId, Name = "Test Customer" };
    }

    public object ApplyDiscount(string orderId, decimal discountPercent)
    {
        try
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return "Order not found";
            }

            if (discountPercent < 0 || discountPercent > 100)
            {
                return false;
            }

            // Apply discount logic...

            return order;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}