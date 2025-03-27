namespace OrderManagement.InjectableVsNewableObjects;
public class OrderShipmentProcessor
{
    private readonly IShippingRateCalculator _shippingRateCalculator;

    public OrderShipmentProcessor(IShippingRateCalculator shippingRateCalculator)
    {
        _shippingRateCalculator = shippingRateCalculator;
    }

    public void ProcessShipment(Order order)
    {
        var shippingCost = _shippingRateCalculator.CalculateRate(
            order.ShippingAddress, 
            order.TotalWeight);
        
        var shipmentInfo = new ShipmentInfo // Newable
        {
            OrderId = order.Id,
            Weight = order.TotalWeight,
            ShippingCost = shippingCost
        };
        
        // Process shipment...
    }
}