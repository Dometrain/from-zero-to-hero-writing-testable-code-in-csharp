namespace Exercises;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int LoyaltyPoints { get; set; }
    public CustomerTier Tier { get; set; }
}