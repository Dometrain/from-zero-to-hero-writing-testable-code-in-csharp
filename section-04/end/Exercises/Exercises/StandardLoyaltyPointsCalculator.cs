namespace Exercises;

public class StandardLoyaltyPointsCalculator : ILoyaltyPointsCalculator
{
    public int CalculatePoints(Order order)
    {
        return (int)(order.TotalAmount / 10);
    }
}