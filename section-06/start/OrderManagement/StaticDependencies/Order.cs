namespace OrderManagement.StaticDependencies;

public class Order
{
    public int Id { get; set; }
    public Address ShippingAddress { get; set; }
    public decimal TotalWeight { get; set; }
    public bool ContainsFragileItems { get; set; }
}