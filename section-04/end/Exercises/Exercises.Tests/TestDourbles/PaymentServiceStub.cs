namespace Exercises.Tests.TestDourbles;

public class PaymentServiceStub : IPaymentService
{
    private readonly bool _paymentSucceeds;

    public PaymentServiceStub(bool paymentSucceeds = true)
    {
        _paymentSucceeds = paymentSucceeds;
    }

    public bool ProcessPayment(PaymentInfo paymentInfo, decimal amount) => _paymentSucceeds;
}