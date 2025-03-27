namespace OrderManagement.CompositionOfNarrowComponents;

public interface IInventoryValidator
{
    bool HasSufficientInventory(int productId, int requestedQuantity);
}