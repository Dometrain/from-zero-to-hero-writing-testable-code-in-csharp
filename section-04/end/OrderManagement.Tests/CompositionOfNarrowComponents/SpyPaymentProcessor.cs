using OrderManagement.CompositionOfNarrowComponents;

namespace OrderManagement.Tests.CompositionOfNarrowComponents;

public class SpyPaymentProcessor : IPaymentProcessor
{
    private readonly Exception? _exceptionToThrow;
    public int ProcessPaymentCallCount { get; private set; }
    public PaymentInfo? LastPaymentInfo { get; private set; }
    public decimal? LastAmount { get; private set; }

    public SpyPaymentProcessor(Exception? exceptionToThrow = null)
    {
        _exceptionToThrow = exceptionToThrow;
    }

    public void ProcessPayment(PaymentInfo paymentInfo, decimal amount)
    {
        ProcessPaymentCallCount++;
        LastPaymentInfo = paymentInfo;
        LastAmount = amount;

        if (_exceptionToThrow != null)
        {
            throw _exceptionToThrow;
        }
    }
}