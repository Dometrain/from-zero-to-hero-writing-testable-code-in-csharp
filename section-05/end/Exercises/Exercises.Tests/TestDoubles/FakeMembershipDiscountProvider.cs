namespace Exercises.Tests.TestDoubles;

internal class FakeMembershipDiscountProvider : IMembershipDiscountProvider
{
    private readonly bool _isEligible;
    private readonly decimal _discountPercentage;

    public FakeMembershipDiscountProvider(bool isEligible = true, decimal discountPercentage = 0.2m)
    {
        _isEligible = isEligible;
        _discountPercentage = discountPercentage;
    }

    public bool IsEligibleForDiscount(Membership membership)
    {
        return _isEligible;
    }

    public decimal GetDiscountPercentage(MembershipLevel level)
    {
        return _discountPercentage;
    }
}