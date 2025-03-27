namespace OrderManagement.AvoidingTestInterference;

public static class DiscountManager
{
    // Global discount that applies to all orders
    public static decimal ActiveDiscountPercentage { get; set; } = 0;
}