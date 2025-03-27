using OrderManagement.Api.Core;
using OrderManagement.Api.Domain;
using OrderManagement.Api.Handlers;
using OrderManagement.Api.Models;

namespace OrderManagement.Api.Infrastructure;

public class ShippingApiClient : IShippingService
{
    private readonly HttpClient _httpClient;

    public ShippingApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ShippingApi");
    }

    public async Task<Result<ShippingEstimateResponse>> EstimateAsync(Order order, Address address)
    {
        var shippingResponse = await _httpClient.PostAsJsonAsync("api/shipping/estimate", new
        {
            OrderId = order.Id,
            Address = address,
            ItemCount = order.Items.Sum(i => i.Quantity)
        });

        if (shippingResponse.IsSuccessStatusCode)
        {
            var shippingEstimate = await shippingResponse.Content
                .ReadFromJsonAsync<ShippingEstimateResponse>();
            
            return Result<ShippingEstimateResponse>.Success(shippingEstimate);
        }
        
        return Result<ShippingEstimateResponse>.Failure(
            new Error(ErrorCodes.ShippingFailure, "Error occured while estimating shipping"));
    }
}