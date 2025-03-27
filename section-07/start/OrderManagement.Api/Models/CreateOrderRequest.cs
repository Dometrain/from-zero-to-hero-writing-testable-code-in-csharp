using OrderManagement.Api.Domain;

namespace OrderManagement.Api.Models;

public class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public List<OrderItemRequest> Items { get; set; }
    public Address ShippingAddress { get; set; }
}