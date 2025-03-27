namespace Exercises;

public class EmailService
{
    public void SendOrderConfirmation(string email, Order order)
    {
        // In a real scenario, this would send an email
        Console.WriteLine($"Order confirmation sent to {email} for order {order.Id}");
    }
}