namespace Exercises.Tests.TestDourbles;

public class InventoryServiceStub : IInventoryService
{
    private readonly bool _hasInventory;

    public InventoryServiceStub(bool hasInventory = true)
    {
        _hasInventory = hasInventory;
    }

    public bool CheckInventory(int productId, int quantity) => _hasInventory;

    public void UpdateInventory(OrderItem item)
    {
    }

    public Product GetProduct(int productId)
    {
        throw new NotImplementedException();
    }
}