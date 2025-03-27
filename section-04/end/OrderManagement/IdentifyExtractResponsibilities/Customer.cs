namespace OrderManagement.IdentifyExtractResponsibilities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int LoyaltyPoints { get; set; }
    public LoyaltyTier LoyaltyTier { get; set; }
}