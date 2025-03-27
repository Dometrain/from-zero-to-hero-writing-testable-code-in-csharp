namespace Exercises;

public class OrderPrinter
{
    public void PrintOrderConfirmation(Order order)
    {
        Console.WriteLine("===== ORDER CONFIRMATION =====");
        Console.WriteLine($"Order #: {order.ReferenceNumber}");
        Console.WriteLine($"Date: {order.CreatedAt}");
        Console.WriteLine($"Customer: {order.Customer.Name}");
        Console.WriteLine("\nItems:");
            
        foreach (var line in order.Lines)
        {
            Console.WriteLine($"  {line.Quantity} x {line.Description} @ {line.UnitPrice:C} = {(line.Quantity * line.UnitPrice):C}");
        }
            
        Console.WriteLine($"\nTotal: {order.Total:C}");
            
        Console.WriteLine("\nShipping Address:");
        Console.WriteLine($"  {order.ShippingAddress.Street}");
        Console.WriteLine($"  {order.ShippingAddress.City}, {order.ShippingAddress.State} {order.ShippingAddress.ZipCode}");
        Console.WriteLine($"  {order.ShippingAddress.Country}");
            
        Console.WriteLine("\nThank you for your order!");
        Console.WriteLine("==============================");
    }
}