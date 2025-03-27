namespace OrderManagement.AvoidingTestInterference;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public CustomerType CustomerType { get; set; } = CustomerType.Regular;
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
}