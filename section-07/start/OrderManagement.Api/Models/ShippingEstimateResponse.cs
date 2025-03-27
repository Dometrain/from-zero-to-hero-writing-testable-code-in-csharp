namespace OrderManagement.Api.Models;

public class ShippingEstimateResponse
{
    public DateTime EstimatedDeliveryDate { get; set; }
    public decimal ShippingCost { get; set; }
}