using OrderManagement.Api.Domain;
using OrderManagement.Api.Handlers;

namespace OrderManagement.Api.Data;

public class ProductRepository : IProductRepository
{
    private readonly OrderDbContext _dbContext;

    public ProductRepository(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products.FindAsync(id);
    }

    public async Task UpdateProductAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }
}