namespace Exercises;

public class PaymentService : IPaymentService
{
    private readonly PaymentProcessor _paymentProcessor;

    public PaymentService(PaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;
    }

    public bool ProcessPayment(PaymentInfo paymentInfo, decimal amount)
    {
        return _paymentProcessor.ProcessPayment(paymentInfo, amount);
    }
}