namespace OrderManagement.Api.Models;

public class OrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}