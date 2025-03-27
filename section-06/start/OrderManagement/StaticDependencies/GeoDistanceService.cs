namespace OrderManagement.StaticDependencies;

public static class GeoDistanceService
{
    public static decimal CalculateDistance(string originZip, string destinationZip)
    {
        // In real code, this would use a mapping service API
        // For this example, we'll return a dummy value
        return 42.5M;
    }
}