using OrderManagement.Api.Domain;

namespace OrderManagement.Api.Handlers;

public interface ICustomerRepository
{
    public Task<Customer?> GetCustomerByIdAsync(int id);
}