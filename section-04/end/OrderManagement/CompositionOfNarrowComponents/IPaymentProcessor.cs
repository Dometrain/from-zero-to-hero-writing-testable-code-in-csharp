namespace OrderManagement.CompositionOfNarrowComponents;

public interface IPaymentProcessor
{
    void ProcessPayment(PaymentInfo paymentInfo, decimal amount);
}