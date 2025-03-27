namespace Exercises;

public interface IPaymentService
{
    bool ProcessPayment(PaymentInfo paymentInfo, decimal amount);
}