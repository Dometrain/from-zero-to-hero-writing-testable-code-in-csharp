using OrderManagement.Api.Core;
using OrderManagement.Api.Models;

namespace OrderManagement.Api.Handlers;

public interface ICreateOrderRequestHandler
{
    Task<Result<OrderDto>> HandleAsync(CreateOrderRequest request);
}