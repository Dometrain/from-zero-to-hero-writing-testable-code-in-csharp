namespace Exercises;

public class OrderService
{
    private readonly IInventoryService _inventoryService;
    private readonly IPricingService _pricingService;
    private readonly IPaymentService _paymentService;
    private readonly ICustomerService _customerService;
    private readonly INotificationService _notificationService;
    private readonly IOrderRepository _orderRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger _logger;

    public OrderService(
        IInventoryService inventoryService,
        IPricingService pricingService,
        IPaymentService paymentService,
        ICustomerService customerService,
        INotificationService notificationService,
        IOrderRepository orderRepository,
        IDateTimeProvider dateTimeProvider,
        ILogger logger)
    {
        _inventoryService = inventoryService;
        _pricingService = pricingService;
        _paymentService = paymentService;
        _customerService = customerService;
        _notificationService = notificationService;
        _orderRepository = orderRepository;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public OrderResult ProcessOrder(Order order, PaymentInfo paymentInfo)
    {
        if (!ValidateInventory(order))
        {
            return FailedResult("Insufficient inventory");
        }
            
        try
        {
            order.TotalAmount = _pricingService.CalculateOrderTotal(order);
            
            if (!_paymentService.ProcessPayment(paymentInfo, order.TotalAmount))
            {
                return FailedResult("Payment failed");
            }
            
            UpdateInventory(order);
            
            _customerService.UpdateLoyalty(order.Customer, order.TotalAmount);
            
            FinalizeOrder(order);
            
            _notificationService.SendOrderNotifications(order);
                
            return SuccessResult(order.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing order: {ex.Message}");
            return FailedResult(ex.Message);
        }
    }

    private bool ValidateInventory(Order order)
    {
        foreach (var item in order.Items)
        {
            if (_inventoryService.CheckInventory(item.ProductId, item.Quantity)) continue;
            
            _logger.LogWarning($"Insufficient inventory for product {item.ProductId}");
            return false;
        }
        return true;
    }

    private void UpdateInventory(Order order)
    {
        foreach (var item in order.Items)
        {
            _inventoryService.UpdateInventory(item);
        }
    }

    private void FinalizeOrder(Order order)
    {
        order.OrderDate = _dateTimeProvider.Now;
        order.Status = "Completed";
        _orderRepository.SaveOrder(order);
    }

    private OrderResult SuccessResult(int orderId)
    {
        return new OrderResult
        {
            Success = true,
            OrderId = orderId
        };
    }

    private OrderResult FailedResult(string errorMessage)
    {
        return new OrderResult
        {
            Success = false,
            Error = errorMessage
        };
    }
}