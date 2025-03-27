namespace OrderManagement.AvoidingTestInterference;

public class OrderCalculator
{
    public decimal CalculateTotal(Order order)
    {
        decimal subtotal = 0;
        foreach (var item in order.Items)
        {
            subtotal += item.Price * item.Quantity;
        }
        
        // Apply the active discount (shared state causing test interference)
        var discount = subtotal * (DiscountManager.ActiveDiscountPercentage / 100m);
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