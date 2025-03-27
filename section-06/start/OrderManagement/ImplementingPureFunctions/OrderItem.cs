namespace OrderManagement.ImplementingPureFunctions;

public class OrderItem
{
    public int ProductId { get; set; }
    public ProductType ProductType { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TaxAmount { get; set; }

}