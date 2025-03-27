namespace Exercises;

public interface IInventoryService
{
    bool CheckInventory(int productId, int quantity);
    void UpdateInventory(OrderItem item);
    Product GetProduct(int productId);
}