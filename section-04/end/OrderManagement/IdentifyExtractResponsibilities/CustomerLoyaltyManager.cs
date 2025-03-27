namespace OrderManagement.IdentifyExtractResponsibilities;

public class CustomerLoyaltyManager
{
    public void UpdateLoyaltyPoints(Customer customer, Order order)
    {
        var points = (int)(order.TotalAmount / 10);
        customer.LoyaltyPoints += points;
        
        if (customer.LoyaltyPoints > 1000)
            customer.LoyaltyTier = LoyaltyTier.Gold;
        else if (customer.LoyaltyPoints > 500)
            customer.LoyaltyTier = LoyaltyTier.Silver;
    }
}