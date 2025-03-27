namespace Exercises;

public class NotificationService : INotificationService
{
    private readonly IEmailService _emailService;
    private readonly ILoyaltyPointsCalculator _loyaltyPointsCalculator;
    private readonly IPushNotificationService _pushNotificationService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public NotificationService(
        IEmailService emailService,
        ILoyaltyPointsCalculator loyaltyPointsCalculator,
        IPushNotificationService pushNotificationService,
        IDateTimeProvider dateTimeProvider)
    {
        _emailService = emailService;
        _loyaltyPointsCalculator = loyaltyPointsCalculator;
        _pushNotificationService = pushNotificationService;
        _dateTimeProvider = dateTimeProvider;
    }

    public void SendOrderNotifications(Order order)
    {
        _emailService.SendOrderConfirmation(order.Customer.Email, order);
        
        var points = _loyaltyPointsCalculator.CalculatePoints(order);
            
        var notification = new OrderNotification
        {
            OrderId = order.Id,
            CustomerName = order.Customer.Name,
            TotalAmount = order.TotalAmount,
            LoyaltyPointsEarned = points,
            OrderDate = _dateTimeProvider.Now // Use injected provider
        };
        
        _pushNotificationService.SendNotification(order.Customer.Id, notification);
    }
}