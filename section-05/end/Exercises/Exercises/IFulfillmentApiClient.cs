namespace Exercises;

public interface IFulfillmentApiClient
{
    Task<bool> NotifyOrderReady(int orderId);
}