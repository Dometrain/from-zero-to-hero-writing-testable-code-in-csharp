namespace Exercises;

public class OrderService
{
    private readonly IMetricsRecorder _metricsRecorder;
    private readonly ShippingCostCalculator? _shippingCalculator;
    private readonly IOrderPrinter _orderPrinter;
    private readonly IConfigProvider _configProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRandomNumberGenerator _randomNumberGenerator;
    private readonly IFulfillmentApiClient _fulfillmentClient;

    public OrderService(
        IMetricsRecorder metricsRecorder,
        ShippingCostCalculator? shippingCalculator,
        IOrderPrinter orderPrinter,
        IConfigProvider configProvider,
        IDateTimeProvider dateTimeProvider,
        IRandomNumberGenerator randomNumberGenerator,
        IFulfillmentApiClient fulfillmentClient)
    {
        _metricsRecorder = metricsRecorder;
        _shippingCalculator = shippingCalculator;
        _orderPrinter = orderPrinter;
        _configProvider = configProvider;
        _dateTimeProvider = dateTimeProvider;
        _randomNumberGenerator = randomNumberGenerator;
        _fulfillmentClient = fulfillmentClient;
    }

    public Order CreateOrder(Customer customer, Address shippingAddress, List<OrderLine> orderLines)
    {
        // Generate the reference number using our abstractions
        var prefix = _configProvider.GetOrderPrefix();
        var sequence = _configProvider.GetOrderStartSequence() + _randomNumberGenerator.Next(1, 1000);
        var timestamp = _dateTimeProvider.Now.ToString("yyyyMMddHHmm");

        var order = new Order
        {
            Id = sequence,
            ReferenceNumber = $"{prefix}-{timestamp}-{sequence}",
            Customer = customer,
            ShippingAddress = shippingAddress,
            Lines = orderLines,
            Status = OrderStatus.Pending,
            CreatedAt = _dateTimeProvider.Now,
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
            _metricsRecorder.RecordOrderCancelled(order, "Processing attempted on cancelled order");
            return false;
        }

        if (order.Status != OrderStatus.Pending)
        {
            _metricsRecorder.RecordOrderCancelled(order, $"Invalid status: {order.Status}");
            return false;
        }

        // Process the order...
        order.Status = OrderStatus.Confirmed;
        _metricsRecorder.RecordOrderProcessed(order);
        _metricsRecorder.IncrementCounter("orders_processed");

        return true;
    }

    public void PrintOrderConfirmation(Order order)
    {
        _orderPrinter.PrintOrderConfirmation(order);
    }

    public async Task<bool> NotifyFulfillmentTeam(Order order)
    {
        return await _fulfillmentClient.NotifyOrderReady(order.Id);
    }
}