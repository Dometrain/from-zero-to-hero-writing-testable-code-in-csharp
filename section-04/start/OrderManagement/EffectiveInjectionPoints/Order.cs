namespace OrderManagement.EffectiveInjectionPoints;

public class Order
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItem> Items { get; set; }
}