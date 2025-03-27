namespace Exercises.Tests.TestDoubles;

public class FakeInventorySystem : IInventorySystem
{
    private Dictionary<int, int> _inventory = new Dictionary<int, int>();
    private List<string> _operations = [];
        
    public FakeInventorySystem(Dictionary<int, int> initialInventory = null)
    {
        if (initialInventory != null)
        {
            _inventory = new Dictionary<int, int>(initialInventory);
        }
        else
        {
            _inventory = new Dictionary<int, int>
            {
                { 101, 50 },
                { 102, 25 },
                { 103, 10 }
            };
        }
    }
        
    public bool CheckStock(int productId, int quantity)
    {
        _operations.Add($"CheckStock: {productId}, {quantity}");
            
        if (!_inventory.ContainsKey(productId))
            return false;
                
        return _inventory[productId] >= quantity;
    }
        
    public bool DeductStock(int productId, int quantity)
    {
        _operations.Add($"DeductStock: {productId}, {quantity}");
            
        if (!CheckStock(productId, quantity))
            return false;
                
        _inventory[productId] -= quantity;
        return true;
    }
        
    public List<string> GetOperations() => _operations;
    public Dictionary<int, int> GetInventory() => _inventory;
}