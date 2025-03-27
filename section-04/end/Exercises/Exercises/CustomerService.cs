namespace Exercises;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger _logger;

    public CustomerService(ICustomerRepository customerRepository, ILogger logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public void UpdateLoyalty(Customer customer, decimal orderAmount)
    {
        var points = CalculateLoyaltyPoints(orderAmount);
        customer.LoyaltyPoints += points;
            
        UpdateCustomerTier(customer);
            
        _customerRepository.SaveCustomer(customer);
        _logger.LogInformation($"Customer {customer.Id} loyalty updated to {customer.Tier}");
    }

    private static int CalculateLoyaltyPoints(decimal orderAmount)
    {
        return (int)(orderAmount / 10);
    }

    private static void UpdateCustomerTier(Customer customer)
    {
        customer.Tier = customer.LoyaltyPoints switch
        {
            > 1000 => CustomerTier.Gold,
            > 500 => CustomerTier.Silver,
            _ => customer.Tier
        };
    }
}