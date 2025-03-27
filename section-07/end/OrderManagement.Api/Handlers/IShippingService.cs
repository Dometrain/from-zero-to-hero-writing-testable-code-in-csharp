using OrderManagement.Api.Core;
using OrderManagement.Api.Domain;
using OrderManagement.Api.Models;

namespace OrderManagement.Api.Handlers;

public interface IShippingService
{
    Task<Result<ShippingEstimateResponse>> EstimateAsync(Order order, Address requestShippingAddress);
}