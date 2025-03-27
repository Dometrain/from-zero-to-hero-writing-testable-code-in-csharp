namespace Exercises;

public class PushNotificationService: IPushNotificationService
{
    public void SendNotification(int customerId, OrderNotification notification)
    {
        // In a real scenario, this would send a push notification
        Console.WriteLine($"Push notification sent to customer {customerId} for order {notification.OrderId}");
    }
}