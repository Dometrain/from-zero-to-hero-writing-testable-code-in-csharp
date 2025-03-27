using OrderManagement.CompositionOfNarrowComponents;

namespace OrderManagement.Tests.CompositionOfNarrowComponents;

public class StubPriceCalculator : IPriceCalculator
{
    private readonly decimal _totalPrice;

    public StubPriceCalculator(decimal totalPrice)
    {
        _totalPrice = totalPrice;
    }

    public decimal Calculate(ShoppingCart cart)
    {
        return _totalPrice;
    }
}