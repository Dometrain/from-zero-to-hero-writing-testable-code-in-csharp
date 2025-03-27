namespace OrderManagement.DealWithConstructorBloat;

public class OrderProcessor
{
    private readonly OrderFinancialServices _orderFinanceServices;
    private readonly InventoryManager _inventoryManager;
    private readonly PaymentProcessor _paymentProcessor;
    private readonly OrderValidator _orderValidator;
    private readonly OrderNotificationService _orderNotificationService;
    
    public OrderProcessor(
        OrderFinancialServices orderFinanceServices,
        InventoryManager inventoryManager,
        PaymentProcessor paymentProcessor,
        OrderValidator orderValidator,
        OrderNotificationService orderNotificationService)
    {
        _orderFinanceServices = orderFinanceServices;
        _inventoryManager = inventoryManager;
        _paymentProcessor = paymentProcessor;
        _orderValidator = orderValidator;
        _orderNotificationService = orderNotificationService;
    }
    
    public OrderResult ProcessOrder(Order order)
    {
        // Validate order
        if (!_orderValidator.ValidateOrder(order))
        {
            return new OrderResult { Success = false, Error = "Invalid order" };
        }

        // Calculate financials
        order.TotalAmount = _orderFinanceServices.GetFinalAmount(order);

        // Process payment
        var paymentSuccessful = _paymentProcessor.ProcessPayment(order, order.TotalAmount);
        if (!paymentSuccessful)
        {
            return new OrderResult { Success = false, Error = "Payment failed" };
        }

        // Update inventory
        _inventoryManager.ReserveInventory(order);

        // Notify stakeholders
        _orderNotificationService.NotifyOrderProcessed(order);

        return new OrderResult { Success = true, OrderId = order.Id };
    }
}