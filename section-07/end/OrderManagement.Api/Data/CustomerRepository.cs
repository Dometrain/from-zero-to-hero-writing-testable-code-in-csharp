using OrderManagement.Api.Domain;
using OrderManagement.Api.Handlers;

namespace OrderManagement.Api.Data;

public class CustomerRepository: ICustomerRepository
{
    private readonly OrderDbContext _dbContext;

    public CustomerRepository(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        return await _dbContext.Customers.FindAsync(id);
    }
}