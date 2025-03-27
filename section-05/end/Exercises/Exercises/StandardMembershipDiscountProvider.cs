namespace Exercises;

public class StandardMembershipDiscountProvider : IMembershipDiscountProvider
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public StandardMembershipDiscountProvider(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public bool IsEligibleForDiscount(Membership membership)
    {
        return membership != null &&
               membership.ExpiresAt > _dateTimeProvider.Now;
    }

    public decimal GetDiscountPercentage(MembershipLevel level)
    {
        return level switch
        {
            MembershipLevel.Elite => 0.5m, // 50% off
            MembershipLevel.Premium => 0.2m, // 20% off
            _ => 0.0m // No discount
        };
    }
}