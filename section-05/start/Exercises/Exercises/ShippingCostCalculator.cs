namespace Exercises;

public class ShippingCostCalculator
{
    private readonly Dictionary<string, decimal> _countryRates = new Dictionary<string, decimal>
    {
        { "US", 5.00m },
        { "CA", 8.00m },
        { "MX", 10.00m },
        { "UK", 15.00m }
    };
        
    public decimal CalculateShippingCost(Order order)
    {
        var baseRate = _countryRates.ContainsKey(order.ShippingAddress.Country) 
            ? _countryRates[order.ShippingAddress.Country] 
            : 20.00m;
            
        // Apply weight factor (assume weight = quantity for simplicity)
        var totalItems = order.Lines.Sum(item => item.Quantity);
        var weightFactor = Math.Ceiling(totalItems / 10.0m);
            
        // Apply membership discount if applicable
        if (order.Customer.Membership.Level == MembershipLevel.Elite && 
            order.Customer.Membership.ExpiresAt > DateTime.Now)
        {
            // Elite members get 50% off shipping
            return (baseRate * weightFactor) * 0.5m;
        }
        else if (order.Customer.Membership.Level == MembershipLevel.Premium &&
                 order.Customer.Membership.ExpiresAt > DateTime.Now)
        {
            // Premium members get 20% off shipping
            return (baseRate * weightFactor) * 0.8m;
        }
            
        return baseRate * weightFactor;
    }
}