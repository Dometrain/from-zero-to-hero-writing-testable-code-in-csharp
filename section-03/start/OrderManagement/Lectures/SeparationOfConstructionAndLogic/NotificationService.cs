namespace OrderManagement.Lectures.SeparationOfConstructionAndLogic;

public class NotificationService
{
    private readonly bool _useHtml;
    private readonly bool _includeTrackingLink;
    
    public NotificationService(string serverType, Order order)
    {
        if (serverType == "Exchange")
        {
            _useHtml = true;
        }
        else
        {
            _useHtml = false;
        }

        if (order.Status == OrderStatus.Shipped)
        {
            _includeTrackingLink = true;
        }
    }

    public async Task SendOrderNotification(string customerEmail)
    {
        // Send notification
    }
}