namespace OrderManagement.EffectiveInjectionPoints;

public interface IPromotionRule
{
    bool IsApplicable(Order order);
    decimal CalculateDiscount(Order order);
}