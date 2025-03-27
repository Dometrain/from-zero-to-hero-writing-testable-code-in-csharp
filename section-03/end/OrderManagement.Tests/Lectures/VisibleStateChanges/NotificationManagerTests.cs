using OrderManagement.Lectures.VisibleStateChanges;

namespace OrderManagement.Tests.Lectures.VisibleStateChanges;

public class NotificationManagerTests
{
    [Fact]
    public void Should_disable_notifications()
    {
        var manager = new NotificationManager();
        
        manager.DisableNotifications();
        
        Assert.False(manager.NotificationsEnabled);
    }
    
    [Fact]
    public void Should_notifications_be_enabled_by_default()
    {
        var manager = new NotificationManager();
        
        Assert.True(manager.NotificationsEnabled);
    }
}