using OrderManagement.Api.Domain;
using OrderManagement.Api.Handlers;

namespace OrderManagement.UnitTests.TestDoubles;

public class InMemoryCustomerRepository : ICustomerRepository
{
    public readonly List<Customer> Customers = new();
    
    public Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return Task.FromResult(Customers.FirstOrDefault(c => c.Id == id));
    }
}