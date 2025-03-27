namespace Exercises.Tests.TestDoubles;

public class FakeFulfillmentApiClient : IFulfillmentApiClient
{
    public bool SuccessResult { get; set; } = true;
    public int? LastOrderId { get; private set; }

    public Task<bool> NotifyOrderReady(int orderId)
    {
        LastOrderId = orderId;
        return Task.FromResult(SuccessResult);
    }
}