namespace OrderManagement.DependencyChains;

public class Customer
{
    private LoyaltyProgram _loyaltyProgram;

    public Customer(LoyaltyProgram loyaltyProgram)
    {
        _loyaltyProgram = loyaltyProgram;
    }

    public LoyaltyProgram GetLoyaltyProgram() => _loyaltyProgram;
}