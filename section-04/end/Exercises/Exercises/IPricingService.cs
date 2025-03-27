namespace Exercises;

public interface IPricingService
{
    decimal CalculateOrderTotal(Order order);
}