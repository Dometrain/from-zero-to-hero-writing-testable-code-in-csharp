namespace Exercises;

public class OrderService
{
    private readonly DiscountCalculator _discountCalculator;
    private readonly TaxCalculator _taxCalculator;
    private readonly PaymentProcessor _paymentProcessor;
    private readonly EmailService _emailService;
    private readonly PushNotificationService _pushNotificationService;
    private readonly List<Product> _inventory;
    private readonly List<Order> _orders;


    public OrderService()
    {
        _discountCalculator = new DiscountCalculator();
        _taxCalculator = new TaxCalculator();
        _paymentProcessor = new PaymentProcessor();
        _emailService = new EmailService();
        _pushNotificationService = new PushNotificationService();
        _inventory = new List<Product>();
        _orders = new List<Order>();
    }
    
    public OrderResult ProcessOrder(Order order, PaymentInfo paymentInfo)
    {
        foreach (var item in order.Items)
        {
            var product = _inventory.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null || product.StockQuantity < item.Quantity)
            {
                return new OrderResult
                {
                    Success = false,
                    Error = $"Insufficient inventory for product {item.ProductId}"
                };
            }
        }

        decimal totalPrice = CalculateOrderTotal(order);

        try
        {
            if (!_paymentProcessor.ProcessPayment(paymentInfo, totalPrice))
            {
                return new OrderResult { Success = false, Error = "Payment failed" };
            }
        }
        catch (Exception ex)
        {
            return new OrderResult { Success = false, Error = ex.Message };
        }

        UpdateInventory(order);

        UpdateCustomerLoyalty(order);

        order.TotalAmount = totalPrice;
        order.OrderDate = DateTime.Now; 
        order.Status = "Completed";
        _orders.Add(order);
        
        SendOrderNotifications(order);

        return new OrderResult { Success = true, OrderId = order.Id };
    }
    
    public decimal CalculateOrderTotal(Order order)
    {
        decimal subtotal = 0;
        foreach (var item in order.Items)
        {
            subtotal += item.Price * item.Quantity;
        }
        
        decimal discount = 0;
        if (order.Customer.Tier == CustomerTier.Gold)
        {
            discount = subtotal * 0.1m; // 10% discount
        }
        else if (order.Customer.Tier == CustomerTier.Silver)
        {
            discount = subtotal * 0.05m; // 5% discount
        }

        // Apply seasonal discount 
        if (DateTime.Now.Month == 12)
        {
            discount += subtotal * 0.05m; // Additional 5% Christmas discount
        }

        // Calculate tax
        decimal tax = _taxCalculator.CalculateTax(subtotal - discount);

        return subtotal - discount + tax;
    }
    
    private void UpdateInventory(Order order)
    {
        foreach (var item in order.Items)
        {
            var product = _inventory.FirstOrDefault(p => p.Id == item.ProductId);
            if (product != null)
            {
                product.StockQuantity -= item.Quantity;
                
                if (product.StockQuantity < 10)
                {
                    Console.WriteLine(
                        $"Low stock alert: Product {product.Id} has only {product.StockQuantity} units left.");
                }
            }
        }
    }
    
    private void UpdateCustomerLoyalty(Order order)
    {
        // Calculate loyalty points
        int points = (int)(order.TotalAmount / 10);
        order.Customer.LoyaltyPoints += points;

        // Update customer tier based on points
        if (order.Customer.LoyaltyPoints > 1000)
        {
            order.Customer.Tier = CustomerTier.Gold;
        }
        else if (order.Customer.LoyaltyPoints > 500)
        {
            order.Customer.Tier = CustomerTier.Silver;
        }
        
        SaveCustomer(order.Customer);
        Console.WriteLine($"Customer {order.Customer.Id} loyalty updated to {order.Customer.Tier}");
    }

    // This would normally involve a database, but we're just keeping it in memory for now
    private void SaveCustomer(Customer customer)
    {
        // In a real implementation, this would save to a database
        Console.WriteLine($"Saving customer {customer.Id} to repository");
    }
    
    private void SendOrderNotifications(Order order)
    {
        _emailService.SendOrderConfirmation(order.Customer.Email, order);
        
        int points = (int)(order.TotalAmount / 10);
        
        var notification = new OrderNotification
        {
            OrderId = order.Id,
            CustomerName = order.Customer.Name,
            TotalAmount = order.TotalAmount,
            LoyaltyPointsEarned = points,
            OrderDate = DateTime.Now 
        };
        
        _pushNotificationService.SendNotification(order.Customer.Id, notification);
    }
}