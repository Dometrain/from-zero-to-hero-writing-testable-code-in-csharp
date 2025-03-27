using OrderManagement.Api.Domain;
using OrderManagement.Api.Handlers;

namespace OrderManagement.UnitTests.TestDoubles;

public class InMemoryOrderRepository : IOrderRepository
{
    public readonly List<Order> Orders = new();
    private int _nextId = 1;
    
    public Task<Order> AddOrderAsync(Order order)
    {
        order.Id = _nextId++;
        Orders.Add(order);
        return Task.FromResult(order);
    }
    
    public Task UpdateOrderAsync(Order order)
    {
        var existingOrder = Orders.FirstOrDefault(o => o.Id == order.Id);
        if (existingOrder != null)
        {
            Orders.Remove(existingOrder);
            Orders.Add(order);
        }
        return Task.CompletedTask;
    }
    
    public Task<Order?> GetOrderByIdAsync(int id)
    {
        return Task.FromResult(Orders.FirstOrDefault(o => o.Id == id));
    }
}