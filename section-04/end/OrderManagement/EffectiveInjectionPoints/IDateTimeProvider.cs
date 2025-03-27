namespace OrderManagement.EffectiveInjectionPoints;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}