namespace Exercises;

public class PricingService : IPricingService
{
    private readonly TaxCalculator _taxCalculator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PricingService(TaxCalculator taxCalculator, IDateTimeProvider dateTimeProvider)
    {
        _taxCalculator = taxCalculator;
        _dateTimeProvider = dateTimeProvider;
    }

    public decimal CalculateOrderTotal(Order order)
    {
        var subtotal = CalculateSubtotal(order);
        var discount = CalculateDiscount(order, subtotal);
        var tax = _taxCalculator.CalculateTax(subtotal - discount);
            
        return subtotal - discount + tax;
    }

    private decimal CalculateSubtotal(Order order)
    {
        return order.Items.Sum(item => item.Price * item.Quantity);
    }

    private decimal CalculateDiscount(Order order, decimal subtotal)
    {
        decimal discount = 0;
        
        discount += CalculateTierDiscount(order.Customer.Tier, subtotal);
        
        discount += CalculateSeasonalDiscount(subtotal);
            
        return discount;
    }

    private decimal CalculateTierDiscount(CustomerTier tier, decimal subtotal)
    {
        return tier switch
        {
            CustomerTier.Gold => subtotal * 0.1m, // 10% discount
            CustomerTier.Silver => subtotal * 0.05m, // 5% discount
            _ => 0
        };
    }

    private decimal CalculateSeasonalDiscount(decimal subtotal)
    {
        if (_dateTimeProvider.Now.Month == 12)
        {
            return subtotal * 0.05m; // Additional 5% Christmas discount
        }
            
        return 0;
    }
}