namespace Exercises;

public interface ICustomerService
{
    void UpdateLoyalty(Customer customer, decimal orderAmount);
}