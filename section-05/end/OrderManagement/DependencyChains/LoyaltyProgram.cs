namespace OrderManagement.DependencyChains;

public class LoyaltyProgram
{
    private decimal _discountRate;

    public LoyaltyProgram(decimal discountRate)
    {
        _discountRate = discountRate;
    }

    public decimal GetDiscountRate() => _discountRate;
}