using OrderManagement.Api.Domain;

namespace OrderManagement.Api.Handlers;

public interface IProductRepository
{
    Task<Product?> GetProductByIdAsync(int id);
    Task UpdateProductAsync(Product product);
}