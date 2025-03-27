namespace OrderManagement.EffectiveInjectionPoints;

public class HolidayPromotionRule : IPromotionRule
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public HolidayPromotionRule(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public bool IsApplicable(Order order)
    {
        return _dateTimeProvider.Now.Month == 12;
    }

    public decimal CalculateDiscount(Order order)
    {
        return order.TotalAmount * 0.1m; // 10% holiday discount
    }
}