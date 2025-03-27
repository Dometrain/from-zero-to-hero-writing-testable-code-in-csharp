namespace OrderManagement.EffectiveInjectionPoints;

public class DiscountService
{
    private readonly IEnumerable<IPromotionRule> _promotionRules;

    public DiscountService(IEnumerable<IPromotionRule> promotionRules)
    {
        _promotionRules = promotionRules;
    }
    public decimal CalculateDiscount(Order order)
    {
        decimal discount = 0;
        
        foreach (var rule in _promotionRules)
        {
            if (rule.IsApplicable(order))
            {
                discount += rule.CalculateDiscount(order);
            }
        }

        return discount;
    }
}