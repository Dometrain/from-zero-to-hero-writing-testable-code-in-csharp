namespace OrderManagement.InjectableVsNewableObjects;
public class OrderShipmentProcessor
{
    public void ProcessShipment(Order order)
    {
        var shippingCalculator = new ShippingRateCalculator();
        decimal shippingCost = shippingCalculator.CalculateRate(
            order.ShippingAddress,
            order.TotalWeight);

        var shipmentInfo = new ShipmentInfo
        {
            OrderId = order.Id,
            Weight = order.TotalWeight,
            ShippingCost = shippingCost
        };

        // Process shipment...
    }
}