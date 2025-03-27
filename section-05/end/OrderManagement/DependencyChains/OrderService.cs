namespace OrderManagement.DependencyChains;

public interface IDiscountProvider
{
    decimal GetDiscountRate();
}

public class LoyaltyProgramDiscountProvider : IDiscountProvider
{
    private readonly LoyaltyProgram _loyaltyProgram;
    
    public LoyaltyProgramDiscountProvider(LoyaltyProgram loyaltyProgram)
    {
        _loyaltyProgram = loyaltyProgram ?? 
                          throw new ArgumentNullException(nameof(loyaltyProgram));
    }
    
    public decimal GetDiscountRate()
    {
        return _loyaltyProgram.GetDiscountRate();
    }
}

public class OrderService
{
    public decimal CalculateOrderDiscount(Order order, IDiscountProvider discountProvider)
    {
        var discountRate = discountProvider.GetDiscountRate();

        return order.TotalAmount * discountRate;
    }
}