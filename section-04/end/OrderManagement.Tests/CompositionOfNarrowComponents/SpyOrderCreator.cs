using OrderManagement.CompositionOfNarrowComponents;

namespace OrderManagement.Tests.CompositionOfNarrowComponents;

public class SpyOrderCreator : IOrderCreator
{
    private readonly int _orderId;
    public ShoppingCart? LastCart { get; private set; }
    public decimal? LastTotalPrice { get; private set; }

    public SpyOrderCreator(int orderId)
    {
        _orderId = orderId;
    }

    public Order CreateOrder(ShoppingCart cart, decimal totalPrice)
    {
        LastCart = cart;
        LastTotalPrice = totalPrice;
        return new Order { Id = _orderId };
    }
}