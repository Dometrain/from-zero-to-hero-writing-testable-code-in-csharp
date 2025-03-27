namespace Exercises;

public class ShippingCostCalculator
{
    private readonly IShippingRateProvider _rateProvider;
    private readonly IMembershipDiscountProvider _discountProvider;

    public ShippingCostCalculator(
        IShippingRateProvider rateProvider,
        IMembershipDiscountProvider discountProvider)
    {
        _rateProvider = rateProvider;
        _discountProvider = discountProvider;
    }

    public decimal CalculateShippingCost(Order order)
    {
        // Get base rate from the provider
        var baseRate = _rateProvider.GetBaseRate(order.ShippingAddress.Country);

        // Apply weight factor (assume weight = quantity for simplicity)
        var totalItems = order.Lines.Sum(item => item.Quantity);
        var weightFactor = Math.Ceiling(totalItems / 10.0m);

        // Calculate standard shipping cost
        var shippingCost = baseRate * weightFactor;

        // Apply membership discount if applicable
        if (_discountProvider.IsEligibleForDiscount(order.Customer.Membership))
        {
            var discountPercentage = _discountProvider.GetDiscountPercentage(
                order.Customer.Membership.Level);

            shippingCost *= (1 - discountPercentage);
        }

        return shippingCost;
    }
}