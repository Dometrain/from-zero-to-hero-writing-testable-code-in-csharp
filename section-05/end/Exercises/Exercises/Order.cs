namespace Exercises;

public class Order
{
    public int Id { get; set; }
    public string ReferenceNumber { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public Address ShippingAddress { get; set; }
    public Customer Customer { get; set; }
    public List<OrderLine> Lines { get; set; } = new List<OrderLine>();
}