namespace OrderManagement.Lectures.VisibleStateChanges;

public class NotificationManager
{
    private List<string> _sentNotifications = new List<string>();
    public bool NotificationsEnabled { get; private set; } = true;
    private Customer _currentCustomer;

    public void DisableNotifications()
    {
        NotificationsEnabled = false;
    }

    public Customer SelectCustomer(string customerId)
    {
        _currentCustomer = new Customer { Id = customerId, Email = "customer@example.com" };
        return _currentCustomer;    
    }

    public bool Process(string notificationType, string message)
    {
        if (!NotificationsEnabled)
        {
            return false;
        }

        if (_currentCustomer == null)
        {
            return false;
        }

        _sentNotifications.Add($"{notificationType}:{_currentCustomer.Id}:{message}");

        Console.WriteLine($"Notification sent to {_currentCustomer.Email}");
        return true;
    }
}