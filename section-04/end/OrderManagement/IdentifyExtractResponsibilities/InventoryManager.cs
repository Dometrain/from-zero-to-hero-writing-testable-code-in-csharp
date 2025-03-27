namespace OrderManagement.IdentifyExtractResponsibilities;

public class InventoryManager
{
    public void UpdateInventory(List<OrderItem> items)
    {
        foreach (var item in items)
        {
            item.Product.StockQuantity -= item.Quantity;
            if (item.Product.StockQuantity < 10)
                item.Product.StockStatus = StockStatus.LowStock;
        }
    }
}