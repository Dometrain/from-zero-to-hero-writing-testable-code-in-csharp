namespace OrderManagement.StaticDependencies;

public class Order
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
}

public enum OrderStatus
{
    Created,
    Processing,
    Shipped,
    Delivered,
    Closed
}