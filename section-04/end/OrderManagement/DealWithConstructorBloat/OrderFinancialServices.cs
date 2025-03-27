namespace OrderManagement.DealWithConstructorBloat;

public class OrderFinancialServices
{
    private readonly DiscountCalculator _discountCalculator;
    private readonly ShippingCalculator _shippingCalculator;
    private readonly TaxCalculator _taxCalculator;

    public OrderFinancialServices(ShippingCalculator shippingCalculator, DiscountCalculator discountCalculator, TaxCalculator taxCalculator)
    {
        _shippingCalculator = shippingCalculator;
        _discountCalculator = discountCalculator;
        _taxCalculator = taxCalculator;
    }

    public decimal GetFinalAmount(Order order)
    {
        var discount = _discountCalculator.CalculateDiscount(order);
        var shipping = _shippingCalculator.CalculateShipping(order);
        var tax = _taxCalculator.CalculateTax(order);
        return order.TotalAmount - discount + tax + shipping;
    }
}