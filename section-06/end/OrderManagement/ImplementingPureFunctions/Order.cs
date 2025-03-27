namespace OrderManagement.ImplementingPureFunctions;

public class Order
{
    public int Id { get; set; }
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public decimal TaxAmount { get; set; }
    public DateTime? TaxCalculatedAt { get; set; }
}