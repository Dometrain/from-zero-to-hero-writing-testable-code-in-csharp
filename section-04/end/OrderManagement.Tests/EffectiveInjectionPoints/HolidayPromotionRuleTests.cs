using OrderManagement.EffectiveInjectionPoints;

namespace OrderManagement.Tests.EffectiveInjectionPoints;

public class NonDecemberDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => new DateTime(2025, 6, 1);
}

public class HolidayPromotionRuleTests
{
    [Fact]
    public void IsApplicable_ShouldReturnTrue_InDecember()
    {
        // Arrange
        var dateTimeProvider = new DecemberDateTimeProvider();
        var rule = new HolidayPromotionRule(dateTimeProvider);
        var order = new Order();

        // Act
        var isApplicable = rule.IsApplicable(order);

        // Assert
        Assert.True(isApplicable);
    }

    [Fact]
    public void IsApplicable_ShouldReturnFalse_OutsideDecember()
    {
        // Arrange
        var dateTimeProvider = new NonDecemberDateTimeProvider();
        var rule = new HolidayPromotionRule(dateTimeProvider);
        var order = new Order();

        // Act
        var isApplicable = rule.IsApplicable(order);

        // Assert
        Assert.False(isApplicable);
    }

    [Fact]
    public void CalculateDiscount_ShouldReturn10Percent_OfOrderTotal()
    {
        // Arrange
        var dateTimeProvider = new DecemberDateTimeProvider();
        var rule = new HolidayPromotionRule(dateTimeProvider);
        var order = new Order { TotalAmount = 100 };

        // Act
        var discount = rule.CalculateDiscount(order);

        // Assert
        Assert.Equal(10m, discount); // 10% of 100
    }
} 