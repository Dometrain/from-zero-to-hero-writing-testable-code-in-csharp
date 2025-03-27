namespace Exercises;

public class OrderNotification
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalAmount { get; set; }
    public int LoyaltyPointsEarned { get; set; }
    public DateTime OrderDate { get; set; }
}