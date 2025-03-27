namespace OrderManagement.CompositionOfNarrowComponents;

public interface IPriceCalculator
{
    decimal Calculate(ShoppingCart cart);
}