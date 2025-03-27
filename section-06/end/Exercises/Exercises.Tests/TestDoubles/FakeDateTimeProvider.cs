namespace Exercises.Tests.TestDoubles;

public class FakeDateTimeProvider : IDateTimeProvider
{
    private DateTime _fixedTime;
        
    public FakeDateTimeProvider(DateTime fixedTime)
    {
        _fixedTime = fixedTime;
    }
        
    public DateTime GetCurrentTime() => _fixedTime;
}