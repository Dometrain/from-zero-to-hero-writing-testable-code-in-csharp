namespace Exercises.Tests.TestDourbles;

public class DateTimeProviderStub : IDateTimeProvider
{
    public DateTime Now => new DateTime(2024, 3, 18);
}