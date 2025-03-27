namespace Exercises;

public class OrderNotificationService
{
    public void SendOrderNotifications(Order order)
    {
        var emailSender = new EmailService();
        emailSender.SendOrderConfirmation(order.Customer.Email, order);
        
        var loyaltyPointsCalculator = new LoyaltyPointsCalculator();
        int points = loyaltyPointsCalculator.CalculatePoints(order);
        
        var notification = new OrderNotification
        {
            OrderId = order.Id,
            CustomerName = order.Customer.Name,
            TotalAmount = order.TotalAmount,
            LoyaltyPointsEarned = points,
            OrderDate = DateTime.Now 
        };
        
        var pushNotificationService = new PushNotificationService();
        pushNotificationService.SendNotification(order.Customer.Id, notification);
    }
}