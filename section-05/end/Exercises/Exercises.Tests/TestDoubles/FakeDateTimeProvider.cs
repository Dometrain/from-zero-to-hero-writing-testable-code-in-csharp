namespace Exercises.Tests.TestDoubles;

public class FakeDateTimeProvider : IDateTimeProvider
{
    public DateTime NowValue { get; set; } = new DateTime(2023, 6, 1, 12, 0, 0);

    public DateTime Now => NowValue;
}