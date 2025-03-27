namespace OrderManagement.ImplementingPureFunctions;

public class OrderTaxCalculator
{
    public decimal CalculateTax(Order order)
    {
        foreach (var item in order.Items)
        {
            // Calculate item-level tax
            var itemTax = item.Price * item.Quantity * GetTaxRate(item.ProductType);

            item.TaxAmount = itemTax;
        }

        var totalTax = order.Items.Sum(item => item.TaxAmount);

        order.TaxAmount = totalTax;

        return totalTax;
    }

    private static decimal GetTaxRate(ProductType productType)
    {
        return productType switch
        {
            ProductType.Food => 0.05m, // 5%
            ProductType.Clothing => 0.08m, // 8%
            ProductType.Electronics => 0.10m, // 10%
            _ => 0.07m
        };
    }
}