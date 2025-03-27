namespace OrderManagement.IdentifyExtractResponsibilities;
public class Order
{
    public int OrderId { get; set; }
    public Customer Customer { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal TotalAmount => CalculateTotalAmount();
    
    private decimal CalculateTotalAmount()
    {
        return Items.Sum(item => item.Price * item.Quantity);
    }
    
}