namespace OrderManagement.StaticDependencies;

public static class DistributionCenterLocator
{
    public static string GetNearestCenter(Address shippingAddress)
    {
        // In real code, this would query a database
        return "DIST-12345";
    }
}
public interface IDistributionCenterFinder
{
    string GetNearestCenter(Address shippingAddress);
}

public class DistributionCenterFinder : IDistributionCenterFinder
{
    public string GetNearestCenter(Address shippingAddress)
    {
        return DistributionCenterLocator.GetNearestCenter(shippingAddress);
    }
}

