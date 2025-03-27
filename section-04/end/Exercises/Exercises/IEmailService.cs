namespace Exercises;

public interface IEmailService
{
    void SendOrderConfirmation(string email, Order order);
}