using OrderManagement.Lectures.ExplicitErrorHandling;

namespace OrderManagement.Tests.Lectures.ExplicitErrorHandling;

public class NotificationServiceTests
{
    [Fact]
    public void Should_return_false_when_email_is_empty()
    {
        var service = new NotificationService();
        var order = new Order();
        var customer = new Customer()
        {
            Email = ""
        };
        
        var deliveryResult = service.SendDeliveryUpdate(order, customer);
        
        Assert.False(deliveryResult.IsSuccess);
        Assert.Equal("Email is required", deliveryResult.Error!.Value.Message);
    }
    
    [Fact]
    public void Should_return_false_when_email_is_invalid()
    {
        var service = new NotificationService();
        var order = new Order();
        var customer = new Customer()
        {
            Email = "gui"
        };
        
        var deliveryResult = service.SendDeliveryUpdate(order, customer);
        
        Assert.False(deliveryResult.IsSuccess);
        Assert.Equal("Invalid email", deliveryResult.Error!.Value.Message);
    }
    
    [Fact]
    public void Should_throw_argument_null_exception_when_order_is_null()
    {
        var service = new NotificationService();
        var customer = new Customer();
        
        var exception = Assert.Throws<ArgumentNullException>(() => 
            service.SendDeliveryUpdate(null!, customer)
            );
        Assert.Equal("order", exception.ParamName);
    }
    [Fact]
    public void Should_throw_argument_null_exception_when_customer_is_null()
    {
        var service = new NotificationService();
        
        var exception = Assert.Throws<ArgumentNullException>(() => 
            service.SendDeliveryUpdate(new Order(), null!));
        Assert.Equal("customer", exception.ParamName);
    }

}