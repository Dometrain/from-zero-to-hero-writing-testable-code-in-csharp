using OrderManagement.CompositionOfNarrowComponents;

namespace OrderManagement.Tests.CompositionOfNarrowComponents;

public class FakeProductRepository : IProductRepository
{
    private readonly Dictionary<int, int> _inventoryLevels = new();

    public void SetInventoryLevel(int productId, int level)
    {
        _inventoryLevels[productId] = level;
    }

    public int GetInventoryLevel(int productId)
    {
        return _inventoryLevels.TryGetValue(productId, out var level) ? level : 0;
    }

    public Product? GetProduct(int productId)
    {
        throw new NotImplementedException();
    }
}