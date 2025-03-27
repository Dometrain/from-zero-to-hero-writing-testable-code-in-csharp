using OrderManagement.Api.Domain;

namespace OrderManagement.Api.Models;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
        
    public OrderItemDto(OrderItem item)
    {
        ProductId = item.ProductId;
        ProductName = item.Product?.Name ?? "Unknown Product";
        Quantity = item.Quantity;
        UnitPrice = item.UnitPrice;
        Subtotal = item.Quantity * item.UnitPrice;
    }
}