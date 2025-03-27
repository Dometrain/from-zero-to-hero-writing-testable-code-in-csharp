namespace OrderManagement.DealWithConstructorBloat;

public class OrderProcessor
{
    private readonly DiscountCalculator _discountCalculator;
    private readonly ShippingCalculator _shippingCalculator;
    private readonly TaxCalculator _taxCalculator;
    private readonly InventoryManager _inventoryManager;
    private readonly PaymentProcessor _paymentProcessor;
    private readonly OrderValidator _orderValidator;
    private readonly CustomerNotifier _customerNotifier;
    private readonly WarehouseNotifier _warehouseNotifier;

    public OrderProcessor(
        DiscountCalculator discountCalculator,
        ShippingCalculator shippingCalculator,
        TaxCalculator taxCalculator,
        InventoryManager inventoryManager,
        PaymentProcessor paymentProcessor,
        OrderValidator orderValidator,
        CustomerNotifier customerNotifier,
        WarehouseNotifier warehouseNotifier)
    {
        _discountCalculator = discountCalculator;
        _shippingCalculator = shippingCalculator;
        _taxCalculator = taxCalculator;
        _inventoryManager = inventoryManager;
        _paymentProcessor = paymentProcessor;
        _orderValidator = orderValidator;
        _customerNotifier = customerNotifier;
        _warehouseNotifier = warehouseNotifier;
    }

    public OrderResult ProcessOrder(Order order)
    {
        // Validate order
        if (!_orderValidator.ValidateOrder(order))
        {
            return new OrderResult { Success = false, Error = "Invalid order" };
        }

        // Calculate financials
        var discount = _discountCalculator.CalculateDiscount(order);
        var shipping = _shippingCalculator.CalculateShipping(order);
        var tax = _taxCalculator.CalculateTax(order);
        var finalAmount = order.TotalAmount - discount + tax + shipping;

        // Process payment
        var paymentSuccessful = _paymentProcessor.ProcessPayment(order, finalAmount);
        if (!paymentSuccessful)
        {
            return new OrderResult { Success = false, Error = "Payment failed" };
        }

        // Update inventory
        _inventoryManager.ReserveInventory(order);

        // Notify stakeholders
        _customerNotifier.NotifyCustomer(order);
        _warehouseNotifier.NotifyWarehouse(order);

        return new OrderResult { Success = true, OrderId = order.Id };
    }
}