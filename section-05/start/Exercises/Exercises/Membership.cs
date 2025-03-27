namespace Exercises;

public class Membership
{
    public int Id { get; set; }
    public MembershipLevel Level { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}