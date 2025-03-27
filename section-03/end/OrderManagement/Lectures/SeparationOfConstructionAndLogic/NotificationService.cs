using OrderManagement.Lectures.ExplicitErrorHandling;

namespace OrderManagement.Lectures.SeparationOfConstructionAndLogic;

public class NotificationFormat
{
    public bool Html { get; private set; }
    public bool IncludeTrackingLink { get; private set; }

    private NotificationFormat(bool useHtml)
    {
        Html = useHtml;
    }
    public static NotificationFormat Create(string serverType)
    {
        return serverType == "Exchange" ? 
            new NotificationFormat(true) : 
            new NotificationFormat(false);
    }
    public void WithTrackingLink()
    {
        IncludeTrackingLink = true;
    }
}
public class NotificationService
{
    public async Task SendOrderNotification(string customerEmail, NotificationFormat format)
    {
        // Send notification
    }
}