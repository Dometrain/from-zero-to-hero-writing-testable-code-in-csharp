using OrderManagement.EffectiveInjectionPoints;

namespace OrderManagement.Tests.EffectiveInjectionPoints;

public class LargeOrderPromotionRuleTests
{
    [Fact]
    public void IsApplicable_ShouldReturnTrue_WhenOrderTotalExceeds1000()
    {
        // Arrange
        var rule = new LargeOrderPromotionRule();
        var order = new Order { TotalAmount = 1500 };

        // Act
        var isApplicable = rule.IsApplicable(order);

        // Assert
        Assert.True(isApplicable);
    }

    [Fact]
    public void IsApplicable_ShouldReturnFalse_WhenOrderTotalIsLessThan1000()
    {
        // Arrange
        var rule = new LargeOrderPromotionRule();
        var order = new Order { TotalAmount = 500 };

        // Act
        var isApplicable = rule.IsApplicable(order);

        // Assert
        Assert.False(isApplicable);
    }

    [Fact]
    public void CalculateDiscount_ShouldReturn5Percent_OfOrderTotal()
    {
        // Arrange
        var rule = new LargeOrderPromotionRule();
        var order = new Order { TotalAmount = 2000 };

        // Act
        var discount = rule.CalculateDiscount(order);

        // Assert
        Assert.Equal(100m, discount); // 5% of 2000
    }
} 