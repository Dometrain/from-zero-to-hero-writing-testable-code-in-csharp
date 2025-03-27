namespace OrderManagement.AvoidingTestInterference;

public class OrderCalculator
{
    private readonly DiscountManager _discountManager;

    public OrderCalculator(DiscountManager discountManager)
    {
        _discountManager = discountManager;
    }
    public decimal CalculateTotal(Order order)
    {
        decimal subtotal = 0;
        foreach (var item in order.Items)
        {
            subtotal += item.Price * item.Quantity;
        }
        
        var discount = subtotal * (_discountManager.GetActiveDiscountPercentage() / 100m);
        subtotal -= discount;
        
        // Apply customer-specific discount
        if (order.CustomerType == CustomerType.Premium)
        {
            var premiumDiscount = subtotal * 0.15m;
            subtotal -= premiumDiscount;
        }
        
        return subtotal;
    }
}