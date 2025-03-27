namespace OrderManagement.ImplementingPureFunctions;

public class OrderTaxCalculator
{
    public decimal ApplyTaxes(Order order)
    {
        foreach (var item in order.Items)
        {
            item.TaxAmount = CalculateItemTax(item);
        }

        order.TaxAmount = CalculateTotalTax(order);
        return order.TaxAmount;
    }

    public decimal CalculateItemTax(OrderItem item)
    {
        return item.Price * item.Quantity * GetTaxRate(item.ProductType);
    }

    public decimal CalculateTotalTax(Order order)
        => order.Items.Sum(CalculateItemTax);

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