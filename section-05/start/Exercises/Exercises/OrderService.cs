namespace Exercises;

public class OrderService
{
    private readonly ShippingCostCalculator _shippingCalculator;
    private readonly OrderPrinter _orderPrinter;
    private readonly OrderConfigProvider _configProvider;
    private readonly HttpClient _httpClient;
        
    public OrderService()
    {
        _shippingCalculator = new ShippingCostCalculator();
        _orderPrinter = new OrderPrinter();
        _configProvider = new OrderConfigProvider();
        _httpClient = new HttpClient();
    }
        
    public Order CreateOrder(Customer customer, Address shippingAddress, List<OrderLine> orderLines)
    {
        var prefix = _configProvider.GetOrderPrefix();
        var sequence = _configProvider.GetOrderStartSequence() + new Random().Next(1, 1000);
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmm");
            
        var order = new Order
        {
            Id = sequence,
            ReferenceNumber = $"{prefix}-{timestamp}-{sequence}",
            Customer = customer,
            ShippingAddress = shippingAddress,
            Lines = orderLines,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.Now,
            Total = orderLines.Sum(item => item.Quantity * item.UnitPrice)
        };
            
        // Add shipping cost
        var shippingCost = _shippingCalculator.CalculateShippingCost(order);
        order.Total += shippingCost;
            
        return order;
    }
        
    public bool ProcessOrder(Order order)
    {
        if (order.Status == OrderStatus.Cancelled)
        {
            Metrics.RecordOrderCancelled(order, "Processing attempted on cancelled order");
            return false;
        }
            
        if (order.Status != OrderStatus.Pending)
        {
            Metrics.RecordOrderCancelled(order, $"Invalid status: {order.Status}");
            return false;
        }
            
        // Process the order...
        order.Status = OrderStatus.Confirmed;
        Metrics.RecordOrderProcessed(order);
        Metrics.IncrementCounter("orders_processed");
            
        return true;
    }
        
    public void PrintOrderConfirmation(Order order)
    {
        _orderPrinter.PrintOrderConfirmation(order);
    }
        
    public async Task<bool> NotifyFulfillmentTeam(Order order)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"https://api.company.com/fulfillment/notify?orderId={order.Id}");
                
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}