namespace Exercises.Tests.TestDourbles;

public class CustomerServiceSpy : ICustomerService
{
    public List<(Customer Customer, decimal Amount)> UpdatedLoyalty { get; } = new();

    public void UpdateLoyalty(Customer customer, decimal orderAmount)
    {
        UpdatedLoyalty.Add((customer, orderAmount));
    }
}