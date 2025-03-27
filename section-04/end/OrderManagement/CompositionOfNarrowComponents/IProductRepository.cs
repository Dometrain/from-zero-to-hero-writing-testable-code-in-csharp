namespace OrderManagement.CompositionOfNarrowComponents;

public interface IProductRepository
{
    int GetInventoryLevel(int productId);

    Product? GetProduct(int productId);
}