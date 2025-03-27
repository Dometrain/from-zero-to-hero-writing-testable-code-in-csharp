namespace OrderManagement.IdentifyExtractResponsibilities;
public class Order
{
    public int OrderId { get; set; }
    public Customer Customer { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal TotalAmount { get; set; }

    public void CalculateTotalAmount()
    {
        TotalAmount = Items.Sum(item => item.Price * item.Quantity);
    }

    public void UpdateCustomerLoyaltyPoints()
    {
        int points = (int)(TotalAmount / 10);
        Customer.LoyaltyPoints += points;

        if (Customer.LoyaltyPoints > 1000)
            Customer.LoyaltyTier = LoyaltyTier.Gold;
        else if (Customer.LoyaltyPoints > 500)
            Customer.LoyaltyTier = LoyaltyTier.Silver;
    }

    public void UpdateInventory()
    {
        foreach (var item in Items)
        {
            item.Product.StockQuantity -= item.Quantity;
            if (item.Product.StockQuantity < 10)
                item.Product.StockStatus = StockStatus.LowStock;
        }
    }
}