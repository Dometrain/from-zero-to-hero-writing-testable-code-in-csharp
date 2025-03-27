namespace OrderManagement.IdentifyExtractResponsibilities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public StockStatus StockStatus { get; set; }
}