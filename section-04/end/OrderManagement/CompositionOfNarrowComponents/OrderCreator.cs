namespace OrderManagement.CompositionOfNarrowComponents;

public class OrderCreator : IOrderCreator
{
    public Order CreateOrder(ShoppingCart cart, decimal totalPrice)
    {
        return new Order 
        { 
            Id = new Random().Next(1000, 9999), // Example ID generation
            CustomerId = cart.CustomerId,
            Items = cart.Items.Select(i => new OrderItem 
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList(),
            TotalAmount = totalPrice
        };
    }
}