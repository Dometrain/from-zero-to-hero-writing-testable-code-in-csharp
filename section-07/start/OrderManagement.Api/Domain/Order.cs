namespace OrderManagement.Api.Domain;

public class Order
{
    public int Id { get; set; }
        
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
        
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
        
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        
    public decimal? DiscountAmount { get; set; }
        
    public DateTime? ShippedDate { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
        
    public Address ShippingAddress { get; set; }
}