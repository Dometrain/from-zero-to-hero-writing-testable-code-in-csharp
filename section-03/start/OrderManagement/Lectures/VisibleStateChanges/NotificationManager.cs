namespace OrderManagement.Lectures.VisibleStateChanges;

public class NotificationManager
{
    private List<string> _sentNotifications = new List<string>();
    private bool _notificationsEnabled = true;
    private Customer _currentCustomer;

    public void DisableNotifications()
    {
        _notificationsEnabled = false;
    }

    public void SelectCustomer(string customerId)
    {
        _currentCustomer = new Customer { Id = customerId, Email = "customer@example.com" };
    }

    public bool Process(string notificationType, string message)
    {
        if (!_notificationsEnabled)
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