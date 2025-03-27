namespace OrderManagement.CompositionOfNarrowComponents;

public class PriceCalculator : IPriceCalculator
{
    private readonly IProductRepository _productRepository;

    public PriceCalculator(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public decimal Calculate(ShoppingCart cart)
    {
        decimal totalPrice = 0;

        foreach (var item in cart.Items)
        {
            var product = _productRepository.GetProduct(item.ProductId);
            totalPrice += product.Price * item.Quantity;

            // Apply discount logic
            if (product.OnSale)
            {
                totalPrice -= product.Price * item.Quantity * 0.1m;
            }
        }

        return totalPrice;
    }
}