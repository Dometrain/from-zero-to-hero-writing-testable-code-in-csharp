using OrderManagement.Api.Domain;

namespace OrderManagement.Api.Models;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Total { get; set; }
    public List<OrderItemDto> Items { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
        
    public OrderDto(Order order)
    {
        Id = order.Id;
        OrderDate = order.OrderDate;
        Status = order.Status;
        Total = order.Items.Sum(i => i.UnitPrice * i.Quantity) - (order.DiscountAmount ?? 0);
        Items = order.Items.Select(i => new OrderItemDto(i)).ToList();
        EstimatedDeliveryDate = order.EstimatedDeliveryDate;
    }
}