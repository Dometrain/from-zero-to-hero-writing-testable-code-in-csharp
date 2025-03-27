namespace OrderManagement.CompositionOfNarrowComponents;
public class OrderCheckoutService
{
    public OrderResult Checkout(ShoppingCart cart, PaymentInfo paymentInfo)
    {
        foreach (var item in cart.Items)
        {
            if (item.Quantity > GetInventoryLevel(item.ProductId))
            {
                return new OrderResult { Success = false, Error = "Not enough inventory" };
            }
        }

        decimal totalPrice = 0;
        foreach (var item in cart.Items)
        {
            var product = GetProduct(item.ProductId);
            totalPrice += product.Price * item.Quantity;

            if (product.OnSale)
            {
                totalPrice -= product.Price * item.Quantity * 0.1m;
            }
        }

        try
        {
            ProcessPayment(paymentInfo, totalPrice);
        }
        catch (Exception ex)
        {
            return new OrderResult { Success = false, Error = ex.Message };
        }

        var order = CreateOrder(cart, totalPrice);

        return new OrderResult { Success = true, OrderId = order.Id };
    }

    // Helper methods
    private int GetInventoryLevel(int productId) => 100;
    private Product GetProduct(int productId) => new Product { Price = 10.99m };
    private void ProcessPayment(PaymentInfo paymentInfo, decimal amount) { }
    private Order CreateOrder(ShoppingCart cart, decimal total) => new Order { Id = 123 };
}