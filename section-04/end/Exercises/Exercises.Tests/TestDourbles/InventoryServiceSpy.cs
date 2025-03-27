namespace Exercises.Tests.TestDourbles;

public class InventoryServiceSpy : IInventoryService
{
    public List<(int ProductId, int Quantity)> CheckedInventory { get; } = new();
    public List<OrderItem> UpdatedItems { get; } = new();

    public bool CheckInventory(int productId, int quantity)
    {
        CheckedInventory.Add((productId, quantity));
        return true;
    }

    public void UpdateInventory(OrderItem item)
    {
        UpdatedItems.Add(item);
    }

    public Product GetProduct(int productId)
    {
        throw new NotImplementedException();
    }
}