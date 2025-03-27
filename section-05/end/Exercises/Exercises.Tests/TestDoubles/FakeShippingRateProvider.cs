namespace Exercises.Tests.TestDoubles;

internal class FakeShippingRateProvider : IShippingRateProvider
{
    private readonly decimal _rateToReturn;

    public FakeShippingRateProvider(decimal rateToReturn = 10.0m)
    {
        _rateToReturn = rateToReturn;
    }

    public decimal GetBaseRate(string countryCode)
    {
        return _rateToReturn;
    }
}