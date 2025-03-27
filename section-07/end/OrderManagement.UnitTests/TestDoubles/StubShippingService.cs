using OrderManagement.Api.Core;
using OrderManagement.Api.Domain;
using OrderManagement.Api.Handlers;
using OrderManagement.Api.Models;

namespace OrderManagement.UnitTests.TestDoubles;

public class StubShippingService : IShippingService
{
    public DateTime EstimatedDeliveryDate { get; set; } = DateTime.UtcNow.AddDays(3);

    public Task<Result<ShippingEstimateResponse>> EstimateAsync(Order order, Address address)
    {
        var response = new ShippingEstimateResponse
        {
            EstimatedDeliveryDate = EstimatedDeliveryDate,
            ShippingCost = 5.99m
        };
        
        return Task.FromResult(Result<ShippingEstimateResponse>.Success(response));
    }
}