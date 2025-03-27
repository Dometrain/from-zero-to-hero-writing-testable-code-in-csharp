namespace Exercises;

public class OrderProcessor
{
    public decimal CalculateOrderTotal(Order order)
    {
        decimal total = 0;

        foreach (var item in order.Items)
        {
            item.LineTotal = item.Price * item.Quantity;

            Console.WriteLine($"Calculated line total for product {item.ProductId}: {item.LineTotal}");

            total += item.LineTotal;
        }

        // Apply current discount if active
        if (PromotionEngine.SpecialOfferActive)
        {
            total *= (1 - (PromotionEngine.CurrentDiscountPercentage / 100));
        }

        order.TotalAmount = total;

        return total;
    }

    public bool ProcessOrder(Order order)
    {
        CalculateOrderTotal(order);

        // Check inventory
        foreach (var item in order.Items)
        {
            if (!InventorySystem.CheckStock(item.ProductId, item.Quantity))
            {
                order.Status = OrderStatus.Cancelled;
                return false;
            }
        }

        foreach (var item in order.Items)
        {
            InventorySystem.DeductStock(item.ProductId, item.Quantity);
        }

        order.Status = OrderStatus.Completed;
        order.ProcessedAt = DateTime.Now;

        AuditLogger.LogOrderProcessed(order);

        return true;
    }
}