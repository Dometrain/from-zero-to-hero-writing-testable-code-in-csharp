namespace OrderManagement.FrameworkCode;

public class OrderNumberGenerator
{
    public string GenerateOrderNumber()
    {
        var random = new Random();
        
        // Generate a unique order number using current timestamp and random numbers
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var randomPart = random.Next(1000, 9999).ToString();
        
        return $"ORD-{timestamp}-{randomPart}";
    }
}