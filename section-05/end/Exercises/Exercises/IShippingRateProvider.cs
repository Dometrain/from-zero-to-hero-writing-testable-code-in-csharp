namespace Exercises;

public interface IShippingRateProvider
{
    decimal GetBaseRate(string countryCode);
}