namespace Exercises;

public class FulfillmentApiClient : IFulfillmentApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    
    public FulfillmentApiClient(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }
    
    public async Task<bool> NotifyOrderReady(int orderId)
    {
        try {
            var response = await _httpClient.GetAsync($"{_baseUrl}/fulfillment/notify?orderId={orderId}");
            return response.IsSuccessStatusCode;
        }
        catch {
            return false;
        }
    }
}