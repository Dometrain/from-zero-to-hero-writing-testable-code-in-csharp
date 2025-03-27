namespace OrderManagement.CompositionOfNarrowComponents;

public class InventoryValidator : IInventoryValidator
{
    private readonly IProductRepository _productRepository;

    public InventoryValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public bool HasSufficientInventory(int productId, int requestedQuantity)
    {
        var available = _productRepository.GetInventoryLevel(productId);
        return available >= requestedQuantity;
    }
}