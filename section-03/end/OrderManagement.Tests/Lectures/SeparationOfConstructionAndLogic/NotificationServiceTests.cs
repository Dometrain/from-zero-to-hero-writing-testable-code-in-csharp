using OrderManagement.Lectures.SeparationOfConstructionAndLogic;

namespace OrderManagement.Tests.Lectures.SeparationOfConstructionAndLogic;

public class NotificationServiceTests
{
    [Fact]
    public void Should_send_as_html_when_using_exchange()
    {
        var notificationFormat = NotificationFormat.Create("Exchange");

        Assert.True(notificationFormat.Html);
    }
}