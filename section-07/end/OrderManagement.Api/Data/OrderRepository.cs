using OrderManagement.Api.Domain;
using OrderManagement.Api.Handlers;

namespace OrderManagement.Api.Data;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _dbContext;

    public OrderRepository(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> AddOrderAsync(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _dbContext.Orders.FindAsync(id);
    }
}