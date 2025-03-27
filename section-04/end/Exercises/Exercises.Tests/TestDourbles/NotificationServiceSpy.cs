namespace Exercises.Tests.TestDourbles;

public class NotificationServiceSpy : INotificationService
{
    public List<Order> NotifiedOrders { get; } = new();

    public void SendOrderNotifications(Order order)
    {
        NotifiedOrders.Add(order);
    }
}