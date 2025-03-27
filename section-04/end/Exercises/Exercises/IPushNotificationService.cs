namespace Exercises;

public interface IPushNotificationService
{
    void SendNotification(int customerId, OrderNotification notification);
}