using OrderManagement.Api.Domain;

namespace OrderManagement.Api.Handlers;

public interface IOrderRepository
{
    Task<Order> AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(int id);
}