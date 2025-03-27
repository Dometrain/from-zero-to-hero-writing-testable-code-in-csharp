namespace Exercises;

public class LoyaltyPointsCalculator
{
    public int CalculatePoints(Order order)
    {
        return (int)(order.TotalAmount / 10);
    }
}