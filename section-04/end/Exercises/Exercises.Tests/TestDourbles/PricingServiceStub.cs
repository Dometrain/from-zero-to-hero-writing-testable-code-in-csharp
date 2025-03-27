namespace Exercises.Tests.TestDourbles;

public class PricingServiceStub : IPricingService
{
    private readonly decimal _total;

    public PricingServiceStub(decimal total = 100m)
    {
        _total = total;
    }

    public decimal CalculateOrderTotal(Order order) => _total;
}