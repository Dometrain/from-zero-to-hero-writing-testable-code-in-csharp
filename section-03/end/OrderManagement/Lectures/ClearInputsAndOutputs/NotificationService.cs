namespace OrderManagement.Lectures.ClearInputsAndOutputs;

public enum NotificationType
{
    Email = 0,
    Sms = 1,
}
public class NotificationService
{
    public bool SendUrgentNotification(Customer customer, NotificationType notificationType, 
        string content,
        string header)
        => SendNotification(customer, notificationType, content, header, true);
    
    public bool SendNotification(Customer customer, NotificationType notificationType, 
        string content, string header)
        => SendNotification(customer, notificationType, content, header, false);
    
    private bool SendNotification(Customer customer, NotificationType notificationType, 
        string content, string header, bool urgent = false)
    {
        if(header == null)
            throw new InvalidOperationException("Header not set");
        
        if (notificationType == NotificationType.Email)
        {
            if (customer.OptOutEmail)
            {
                return false;
            }
                
            // Send email logic...
            Console.WriteLine($"Email sent to {customer.Email} with data: {content}");
            return true;
        }

        if (notificationType == NotificationType.Sms || urgent)
        {
            // Send sms logic...
            Console.WriteLine($"SMS sent to {customer.Phone} with data: {content}");
            return true;
        }

        return false; // Unknown type
    }
    
}