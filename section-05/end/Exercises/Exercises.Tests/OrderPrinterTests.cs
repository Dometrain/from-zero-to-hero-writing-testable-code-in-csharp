namespace Exercises.Tests;

public class OrderPrinterTests
{
    [Fact]
    public void StringOrderPrinter_PrintsOrderDetails()
    {
        // Arrange
        var printer = new StringOrderPrinter();
        var order = CreateSampleOrder();

        // Act
        printer.PrintOrderConfirmation(order);
        var output = printer.GetOutput();

        // Assert
        Assert.Contains("ORDER CONFIRMATION", output);
        Assert.Contains("Order #: REF-123", output);
        Assert.Contains("Customer: John Doe", output);
        Assert.Contains("Test Product", output);
        Assert.Contains("Shipping Address:", output);
        Assert.Contains("123 Main St", output);
        Assert.Contains("Thank you for your order!", output);
    }

    private static Order CreateSampleOrder()
    {
        return new Order
        {
            Id = 1,
            ReferenceNumber = "REF-123",
            CreatedAt = new DateTime(2023, 6, 1),
            Total = 49.98m,
            Customer = new Customer
            {
                Name = "John Doe"
            },
            ShippingAddress = new Address
            {
                Street = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "12345",
                Country = "US"
            },
            Lines = new List<OrderLine>
            {
                new OrderLine
                {
                    Id = 101,
                    Description = "Test Product",
                    Quantity = 2,
                    UnitPrice = 24.99m
                }
            }
        };
    }
}