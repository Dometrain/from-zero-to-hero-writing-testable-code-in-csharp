namespace OrderManagement.EffectiveInjectionPoints;

public class DiscountService
{
    public decimal CalculateDiscount(Order order)
    {
        decimal discount = 0;

        if (DateTime.Now.Month == 12)
        {
            // Holiday promotion
            discount += order.TotalAmount * 0.1m;
        }

        if (order.TotalAmount > 1000)
        {
            discount += order.TotalAmount * 0.05m;
        }

        return discount;
    }
}