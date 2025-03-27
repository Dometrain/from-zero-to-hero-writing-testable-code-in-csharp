namespace Exercises;

public class OrderCalculator
{
    public decimal CalculateOrderTotal(Order order, decimal discountPercentage)
    {
        decimal total = 0;

        foreach (var item in order.Items)
        {
            var lineTotal = item.Price * item.Quantity;
            total += lineTotal;
        }

        // Apply discount if needed
        if (discountPercentage > 0)
        {
            total *= (1 - (discountPercentage / 100));
        }

        return total;
    }
}