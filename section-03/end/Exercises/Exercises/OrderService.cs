namespace Exercises;
public class OrderService
{
    private readonly List<Product> _productInventory;
    private readonly List<Order> _orders;
    private readonly string _storeLocation;

    public OrderService(string storeLocation)
    {
        _productInventory = new List<Product>();
        _orders = new List<Order>();
        _storeLocation = storeLocation ?? throw new ArgumentNullException(nameof(storeLocation));
    }

    public Result<Order> ProcessOrder(string orderId, OrderStatus newStatus)
    {
        if (string.IsNullOrEmpty(orderId))
            return Result<Order>.Failure(new Error("Order ID cannot be empty"));

        var order = _orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
            return Result<Order>.Failure(new Error($"Order with ID {orderId} not found"));

        order.Status = newStatus;

        return Result<Order>.Success(order);
    }

    public Result<Order> CreateOrder(Customer customer, List<OrderItem> items, bool priority = false,
        string? notes = null)
    {
        if (customer == null)
            return Result<Order>.Failure(new Error("Customer cannot be null"));

        if (items == null || !items.Any())
            return Result<Order>.Failure(new Error("Order must contain at least one item"));

        var inventoryErrors = new List<string>();
        foreach (var item in items)
        {
            var product = _productInventory.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null)
            {
                inventoryErrors.Add($"Product with ID {item.ProductId} not found");
                continue;
            }

            if (product.StockQuantity < item.Quantity)
            {
                inventoryErrors.Add($"Insufficient stock for product {item.ProductId}");
            }
        }

        if (inventoryErrors.Count != 0)
            return Result<Order>.Failure(new Error(string.Join("; ", inventoryErrors)));

        foreach (var item in items)
        {
            var product = _productInventory.First(p => p.Id == item.ProductId);
            product.StockQuantity -= item.Quantity;
        }

        var order = new Order
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = customer.Id,
            Items = items,
            OrderDate = DateTime.UtcNow,
            Status = priority ? OrderStatus.Processing : OrderStatus.Pending
        };

        _orders.Add(order);

        return Result<Order>.Success(order);
    }
    
    public Result<Order> ApplyDiscount(string orderId, decimal discountPercent)
    {
        if (string.IsNullOrEmpty(orderId))
            return Result<Order>.Failure(new Error("Order ID cannot be empty"));

        if (discountPercent is < 0 or > 100)
            return Result<Order>.Failure(new Error("Discount percentage must be between 0 and 100"));
        
        var order = _orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
            return Result<Order>.Failure(new Error($"Order with ID {orderId} not found"));
        
        var totalBeforeDiscount = order.Items.Sum(i =>
        {
            var product = _productInventory.FirstOrDefault(p => p.Id == i.ProductId);
            return product != null ? product.Price * i.Quantity : 0;
        });

        order.Total = totalBeforeDiscount;
        order.DiscountAmount =totalBeforeDiscount * (discountPercent / 100); 
        
        return Result<Order>.Success(order);
    }

    public Result<Customer> GetCustomer(string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
            return Result<Customer>.Failure(new Error("Customer ID cannot be empty"));

        // This would typically be a database lookup. Coming up later in the course.
        var customer = new Customer { Id = customerId, Name = "Test Customer" };
        return Result<Customer>.Success(customer);
    }

    public bool IsExpressShippingAvailable()
        => _storeLocation is "Warehouse" or "Mall";
    
    public void AddProductToInventory(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _productInventory.Add(product);
    }
}