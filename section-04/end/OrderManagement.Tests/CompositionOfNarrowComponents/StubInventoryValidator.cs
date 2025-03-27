using OrderManagement.CompositionOfNarrowComponents;

namespace OrderManagement.Tests.CompositionOfNarrowComponents;

public class StubInventoryValidator : IInventoryValidator
{
    private bool _hasInventory;

    public void SetHasInventory(bool hasInventory)
    {
        _hasInventory = hasInventory;
    }

    public bool HasSufficientInventory(int productId, int requestedQuantity)
    {
        return _hasInventory;
    }
}