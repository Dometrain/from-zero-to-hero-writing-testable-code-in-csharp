namespace Exercises;

public class StringOrderPrinter : IOrderPrinter
{
    private readonly StringWriter _writer = new StringWriter();

    public void PrintOrderConfirmation(Order order)
    {
        _writer.WriteLine("===== ORDER CONFIRMATION =====");
        _writer.WriteLine($"Order #: {order.ReferenceNumber}");
        _writer.WriteLine($"Date: {order.CreatedAt}");
        _writer.WriteLine($"Customer: {order.Customer.Name}");
        _writer.WriteLine("\nItems:");

        foreach (var line in order.Lines)
        {
            _writer.WriteLine(
                $"  {line.Quantity} x {line.Description} @ {line.UnitPrice:C} = {(line.Quantity * line.UnitPrice):C}");
        }

        _writer.WriteLine($"\nTotal: {order.Total:C}");

        _writer.WriteLine("\nShipping Address:");
        _writer.WriteLine($"  {order.ShippingAddress.Street}");
        _writer.WriteLine(
            $"  {order.ShippingAddress.City}, {order.ShippingAddress.State} {order.ShippingAddress.ZipCode}");
        _writer.WriteLine($"  {order.ShippingAddress.Country}");

        _writer.WriteLine("\nThank you for your order!");
        _writer.WriteLine("==============================");
    }

    public string GetOutput()
    {
        return _writer.ToString();
    }
}