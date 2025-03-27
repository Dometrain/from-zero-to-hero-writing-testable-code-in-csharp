namespace Exercises;

public class CustomerRepository : ICustomerRepository
{
    private readonly ILogger _logger;

    public CustomerRepository(ILogger logger)
    {
        _logger = logger;
    }

    public void SaveCustomer(Customer customer)
    {
        // In a real implementation, this would save to a database
        _logger.LogInformation($"Customer {customer.Id} saved");
    }
}