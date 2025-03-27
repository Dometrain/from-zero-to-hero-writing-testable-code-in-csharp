using OrderManagement.Lectures.ClearInputsAndOutputs;

namespace OrderManagement.Tests.Lectures.ClearInputsAndOutputs;

public class NotificationServiceTests
{
    [Fact]
    public void Should_return_false_when_customer_opt_out_from_emails()
    {
        var service = new NotificationService();
        var customer = new Customer()
        {
            OptOutEmail = true
        };

        var notificationSent = service.SendNotification(customer, NotificationType.Email, 
            "Order created with number 50",
            "Order received");
        
        Assert.False(notificationSent);
    }
}