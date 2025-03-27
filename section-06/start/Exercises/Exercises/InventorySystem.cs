namespace Exercises;

public static class InventorySystem
{
    private static readonly Dictionary<int, int> Inventory = new()
    {
        { 101, 50 },
        { 102, 25 },
        { 103, 10 }
    };

    public static bool CheckStock(int productId, int quantity)
    {
        if (!Inventory.ContainsKey(productId))
            return false;

        return Inventory[productId] >= quantity;
    }

    public static bool DeductStock(int productId, int quantity)
    {
        if (!CheckStock(productId, quantity))
            return false;

        Inventory[productId] -= quantity;
        return true;
    }
}