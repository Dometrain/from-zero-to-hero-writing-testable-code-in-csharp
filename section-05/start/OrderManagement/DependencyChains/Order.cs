namespace OrderManagement.DependencyChains;

public class Order
{
    public int Id{ get; private set; }
    public decimal TotalAmount { get; private set; }
    private Customer _customer;

    public Order(int id, Customer customer, decimal totalAmount)
    {
        Id = id;
        _customer = customer;
        TotalAmount = totalAmount;
    }

    public Customer GetCustomer() => _customer;
}