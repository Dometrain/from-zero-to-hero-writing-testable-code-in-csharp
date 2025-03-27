namespace OrderManagement.DependencyChains;

public class OrderService
{
    public decimal CalculateOrderDiscount(Order order)
    {
        var discountRate = order
            .GetCustomer()
            .GetLoyaltyProgram()
            .GetDiscountRate();

        return order.TotalAmount * discountRate;
    }
}