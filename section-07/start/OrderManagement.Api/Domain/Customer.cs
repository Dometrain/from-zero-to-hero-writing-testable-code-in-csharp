namespace OrderManagement.Api.Domain;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
        
    public List<Order> Orders { get; set; } = new List<Order>();
}