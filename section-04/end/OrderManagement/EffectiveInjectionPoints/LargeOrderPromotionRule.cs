namespace OrderManagement.EffectiveInjectionPoints;

public class LargeOrderPromotionRule : IPromotionRule
{
    public bool IsApplicable(Order order)
    {
        return order.TotalAmount > 1000;
    }

    public decimal CalculateDiscount(Order order)
    {
        return order.TotalAmount * 0.05m; // 5% large order discount
    }
}