namespace Exercises;

public interface IMembershipDiscountProvider
{
    bool IsEligibleForDiscount(Membership membership);
    decimal GetDiscountPercentage(MembershipLevel level);
}