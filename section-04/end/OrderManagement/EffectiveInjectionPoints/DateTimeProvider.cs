namespace OrderManagement.EffectiveInjectionPoints;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}