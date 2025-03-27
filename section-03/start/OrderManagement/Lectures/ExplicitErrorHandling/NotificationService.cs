namespace OrderManagement.Lectures.ExplicitErrorHandling;

public class NotificationService
{
    public bool SendDeliveryUpdate(Order order, Customer customer)
    {
        if (customer == null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(customer.Email))
        {
            return false;
        }
        
        if(order == null)
            throw new Exception();

        if (!customer.Email.Contains("@"))
        {
            throw new Exception();
        }

        // Send notification here
        
        return true;
    }
}