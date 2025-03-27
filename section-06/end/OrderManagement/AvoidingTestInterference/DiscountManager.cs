namespace OrderManagement.AvoidingTestInterference;

public class DiscountManager
{
    private decimal _activeDiscountPercentage = 0;
    
    public decimal GetActiveDiscountPercentage() => _activeDiscountPercentage;
    
    public void SetActiveDiscountPercentage(decimal percentage) => _activeDiscountPercentage = percentage;
    
}