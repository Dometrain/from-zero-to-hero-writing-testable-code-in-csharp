namespace OrderManagement.StaticDependencies;

public class ShippingCalculator
{
    private readonly IDistanceCalculator _distanceCalculator;
    private readonly IDistributionCenterFinder _distributionCenterFinder;

    public ShippingCalculator(IDistanceCalculator distanceCalculator,
        IDistributionCenterFinder distributionCenterFinder)
    {
        _distanceCalculator = distanceCalculator;
        _distributionCenterFinder = distributionCenterFinder;
    }

    public decimal CalculateShippingCost(Order order)
    {
        var distanceInMiles = 
            _distanceCalculator.CalculateDistance(order.ShippingAddress.ZipCode, 
                _distributionCenterFinder.GetNearestCenter(order.ShippingAddress)
        );
        
        var baseCost = 5.00m;
        
        // Weight surcharge
        if (order.TotalWeight > 20)
        {
            baseCost += (order.TotalWeight - 20) * 0.25m;
        }
        
        // Distance cost
        baseCost += distanceInMiles * 0.1m;
        
        // Special handling for fragile items
        if (order.ContainsFragileItems)
        {
            baseCost *= 1.15m; // 15% surcharge
        }
        
        return Math.Round(baseCost, 2);
    }
}