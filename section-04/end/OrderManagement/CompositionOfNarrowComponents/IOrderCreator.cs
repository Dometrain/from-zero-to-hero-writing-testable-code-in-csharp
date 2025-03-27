namespace OrderManagement.CompositionOfNarrowComponents;

public interface IOrderCreator
{
    Order CreateOrder(ShoppingCart cart, decimal totalPrice);
}