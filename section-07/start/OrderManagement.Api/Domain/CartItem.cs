namespace OrderManagement.Api.Domain;

public class CartItem
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsCheckedOut { get; set; }
}