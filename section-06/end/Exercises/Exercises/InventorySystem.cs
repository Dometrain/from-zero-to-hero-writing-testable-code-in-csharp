namespace Exercises;

public class InventorySystem : IInventorySystem
{
    private readonly Dictionary<int, int> _inventory;

    public InventorySystem()
    {
        _inventory = new Dictionary<int, int>
        {
            { 101, 50 },
            { 102, 25 },
            { 103, 10 }
        };
    }

    public bool CheckStock(int productId, int quantity)
    {
        if (!_inventory.ContainsKey(productId))
            return false;

        return _inventory[productId] >= quantity;
    }

    public bool DeductStock(int productId, int quantity)
    {
        if (!CheckStock(productId, quantity))
            return false;

        _inventory[productId] -= quantity;
        return true;
    }
}