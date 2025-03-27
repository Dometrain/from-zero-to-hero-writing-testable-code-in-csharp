using Exercises.Tests.TestDoubles;

namespace Exercises.Tests;

public class ShippingCalculatorTests
{
    [Fact]
    public void CalculateShippingCost_NoDiscount_ReturnsBaseRateTimesWeightFactor()
    {
        // Arrange
        var rateProvider = new FakeShippingRateProvider(10.0m);
        var discountProvider = new FakeMembershipDiscountProvider(isEligible: false);

        var calculator = new ShippingCostCalculator(rateProvider, discountProvider);

        var order = new Order
        {
            ShippingAddress = new Address { Country = "US" },
            Customer = new Customer
            {
                Membership = new Membership { Level = MembershipLevel.Standard }
            },
            Lines = new List<OrderLine>
            {
                new OrderLine { Quantity = 5 },
                new OrderLine { Quantity = 3 }
            }
        };

        // Act
        var shippingCost = calculator.CalculateShippingCost(order);

        // Assert
        Assert.Equal(10.0m, shippingCost);
    }

    [Fact]
    public void CalculateShippingCost_WithDiscount_AppliesDiscountCorrectly()
    {
        // Arrange
        var rateProvider = new FakeShippingRateProvider(10.0m);
        var discountProvider = new FakeMembershipDiscountProvider(isEligible: true, discountPercentage: 0.5m);

        var calculator = new ShippingCostCalculator(rateProvider, discountProvider);

        var order = new Order
        {
            ShippingAddress = new Address { Country = "US" },
            Customer = new Customer
            {
                Membership = new Membership { Level = MembershipLevel.Elite }
            },
            Lines = new List<OrderLine>
            {
                new OrderLine { Quantity = 15 },
                new OrderLine { Quantity = 10 }
            }
        };

        // Act
        var shippingCost = calculator.CalculateShippingCost(order);

        // Assert
        Assert.Equal(15.0m, shippingCost);
    }

    [Fact]
    public void StandardMembershipDiscountProvider_ExpiredMembership_ReturnsNotEligible()
    {
        // Arrange
        var fixedDate = new DateTime(2023, 6, 1);
        var dateProvider = new FakeDateTimeProvider { NowValue = fixedDate };

        var discountProvider = new StandardMembershipDiscountProvider(dateProvider);

        var expiredMembership = new Membership
        {
            Level = MembershipLevel.Elite,
            ExpiresAt = new DateTime(2023, 5, 1) // Expired
        };

        // Act
        var isEligible = discountProvider.IsEligibleForDiscount(expiredMembership);

        // Assert
        Assert.False(isEligible);
    }

    [Fact]
    public void StandardMembershipDiscountProvider_ActiveMembership_ReturnsCorrectDiscounts()
    {
        // Arrange
        var fixedDate = new DateTime(2023, 6, 1);
        var dateProvider = new FakeDateTimeProvider { NowValue = fixedDate };

        var discountProvider = new StandardMembershipDiscountProvider(dateProvider);

        // Act & Assert
        Assert.Equal(0.5m, discountProvider.GetDiscountPercentage(MembershipLevel.Elite));
        Assert.Equal(0.2m, discountProvider.GetDiscountPercentage(MembershipLevel.Premium));
        Assert.Equal(0.0m, discountProvider.GetDiscountPercentage(MembershipLevel.Standard));
    }
}

// Test implementation for membership discounts