namespace Exercises;

public interface IInventorySystem
{
    bool CheckStock(int productId, int quantity);
    bool DeductStock(int productId, int quantity);
}