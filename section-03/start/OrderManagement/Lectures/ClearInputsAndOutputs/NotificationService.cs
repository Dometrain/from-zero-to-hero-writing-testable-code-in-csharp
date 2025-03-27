namespace OrderManagement.Lectures.ClearInputsAndOutputs;

public class NotificationService
{
    private string _header;
    
    public bool CreateNotification(Customer customer, int notificationType, string data, bool? urgent = null)
    {
        if(_header == null)
            throw new InvalidOperationException("Header not set");
        
        if (notificationType == 0) // Email
        {
            if (customer.OptOutEmail)
            {
                return false;
            }
                
            // Send email logic...
            Console.WriteLine($"Email sent to {customer.Email} with data: {data}");
            return true;
        }

        if (notificationType == 1  || urgent.HasValue && urgent.Value) // SMS
        {
            // Send sms logic...
            Console.WriteLine($"SMS sent to {customer.Phone} with data: {data}");
            return true;
        }

        return false; // Unknown type
    }

    public void CreateHeader(string header)
    {
        _header = header;
    }
}