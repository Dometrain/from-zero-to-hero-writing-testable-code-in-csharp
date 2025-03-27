namespace OrderManagement.InjectableVsNewableObjects;

public class ShippingRateCalculator
{
    public decimal CalculateRate(Address address, decimal weight)
    {
        // Complex business logic to calculate shipping rates
        // based on distance, weight, and carrier rates
        return weight * 0.5m + 5.99m;
    }
}