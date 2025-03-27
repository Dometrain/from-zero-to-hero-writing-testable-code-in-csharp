namespace OrderManagement.CompositionOfNarrowComponents;

public class PaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(PaymentInfo paymentInfo, decimal amount)
    {
        // Payment processing logic
    }
}