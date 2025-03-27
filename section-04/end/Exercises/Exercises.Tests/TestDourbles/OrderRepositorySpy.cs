namespace Exercises.Tests.TestDourbles;

public class OrderRepositorySpy : IOrderRepository
{
    public List<Order> SavedOrders { get; } = new();

    public void SaveOrder(Order order)
    {
        SavedOrders.Add(order);
    }

    public Order GetOrder(int id)
    {
        return SavedOrders.First(o => o.Id == id);
    }
}