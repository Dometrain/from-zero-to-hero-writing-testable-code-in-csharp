namespace Exercises;

public class PaymentProcessor
{
    public bool ProcessPayment(PaymentInfo paymentInfo, decimal amount)
    {
        // Validate payment info
        if (string.IsNullOrEmpty(paymentInfo.CardNumber) ||
            string.IsNullOrEmpty(paymentInfo.ExpiryDate) ||
            string.IsNullOrEmpty(paymentInfo.Cvv))
        {
            return false;
        }

        // Basic card validation
        if (!IsValidCreditCard(paymentInfo.CardNumber))
        {
            return false;
        }

        // In a real scenario, this would connect to a payment gateway
        return true;
    }

    private bool IsValidCreditCard(string cardNumber)
    {
        // Simplified validation for example purposes
        return cardNumber.Length >= 13 && cardNumber.Length <= 16 && cardNumber.All(char.IsDigit);
    }
}