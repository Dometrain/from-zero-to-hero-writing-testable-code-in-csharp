namespace Exercises;

public class DiscountCalculator
{
    public decimal CalculateDiscount(Order order)
    {
        decimal discount = 0;

        // Customer tier discount
        if (order.Customer.Tier == CustomerTier.Gold)
        {
            discount += order.TotalAmount * 0.1m;
        }
        else if (order.Customer.Tier == CustomerTier.Silver)
        {
            discount += order.TotalAmount * 0.05m;
        }

        // Seasonal discount
        if (DateTime.Now.Month == 12)
        {
            discount += order.TotalAmount * 0.05m;
        }

        return discount;
    }
}