namespace Exercises;

public class CountryShippingRateProvider : IShippingRateProvider
{
    private readonly Dictionary<string, decimal> _countryRates = new Dictionary<string, decimal>
    {
        { "US", 5.00m },
        { "CA", 8.00m },
        { "MX", 10.00m },
        { "UK", 15.00m }
    };

    public decimal GetBaseRate(string countryCode)
    {
        return _countryRates.ContainsKey(countryCode)
            ? _countryRates[countryCode]
            : 20.00m;
    }
}