namespace OrderManagement.StaticDependencies;

public static class DistributionCenterLocator
{
    public static string GetNearestCenter(Address shippingAddress)
    {
        // In real code, this would query a database
        return "DIST-12345";
    }
}