using OrderManagement.EffectiveInjectionPoints;

namespace OrderManagement.Tests.EffectiveInjectionPoints;

public class DecemberDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => new DateTime(2025, 12, 1);
}