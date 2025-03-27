namespace OrderManagement.CompositionOfNarrowComponents;

public class OrderCheckoutService
{
    private readonly IInventoryValidator _inventoryValidator;
    private readonly IPriceCalculator _priceCalculator;
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IOrderCreator _orderCreator;

    public OrderCheckoutService(IInventoryValidator inventoryValidator, IPriceCalculator priceCalculator,
        IPaymentProcessor paymentProcessor, IOrderCreator orderCreator)
    {
        _inventoryValidator = inventoryValidator;
        _priceCalculator = priceCalculator;
        _paymentProcessor = paymentProcessor;
        _orderCreator = orderCreator;
    }

    public OrderResult Checkout(ShoppingCart cart, PaymentInfo paymentInfo)
    {
        foreach (var item in cart.Items)
        {
            if (!_inventoryValidator.HasSufficientInventory(item.ProductId, item.Quantity))
            {
                return new OrderResult { Success = false, Error = "Not enough inventory" };
            }
        }

        decimal totalPrice = _priceCalculator.Calculate(cart);
        
        try
        {
            _paymentProcessor.ProcessPayment(paymentInfo, totalPrice);
        }
        catch (Exception ex)
        {
            return new OrderResult { Success = false, Error = ex.Message };
        }

        // Order creation logic
        var order = _orderCreator.CreateOrder(cart, totalPrice);

        return new OrderResult { Success = true, OrderId = order.Id };
    }

    // Helper methods
    private Order CreateOrder(ShoppingCart cart, decimal total) => new Order { Id = 123 };
}