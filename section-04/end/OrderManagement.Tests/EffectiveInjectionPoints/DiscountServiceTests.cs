using OrderManagement.EffectiveInjectionPoints;

namespace OrderManagement.Tests.EffectiveInjectionPoints;

public class DiscountServiceTests
{
    [Fact]
    public void Should_accumulate_discounts_when_multiple_rules_apply()
    {
        // Arrange
        var dateTimeProvider = new DecemberDateTimeProvider();
        var service = new DiscountService([
            new HolidayPromotionRule(dateTimeProvider),
            new LargeOrderPromotionRule()
        ]);
        var order = new Order()
        {
            TotalAmount = 2000 // Large enough to trigger both discounts
        };

        // Act
        var discount = service.CalculateDiscount(order);

        // Assert
        const int expectedDiscount = 200 + 100; // 10% holiday (200) + 5% large order (100)
        Assert.Equal(expectedDiscount, discount);
    }
}