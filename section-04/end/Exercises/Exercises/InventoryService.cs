namespace Exercises;

public class InventoryService : IInventoryService
{
    private readonly List<Product> _inventory;
    private readonly ILogger _logger;

    public InventoryService(List<Product> inventory, ILogger logger)
    {
        _inventory = inventory;
        _logger = logger;
    }

    public bool CheckInventory(int productId, int quantity)
    {
        var product = GetProduct(productId);
        return product.StockQuantity >= quantity;
    }

    public void UpdateInventory(OrderItem item)
    {
        var product = GetProduct(item.ProductId);
        product.StockQuantity -= item.Quantity;
        
        if (product.StockQuantity < 10)
        {
            _logger.LogWarning($"Low stock alert: Product {product.Id} has only {product.StockQuantity} units left.");
        }
    }

    public Product GetProduct(int productId)
    {
        return _inventory.FirstOrDefault(p => p.Id == productId);
    }
}