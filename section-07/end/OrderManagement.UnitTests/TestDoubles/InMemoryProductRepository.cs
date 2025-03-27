using OrderManagement.Api.Domain;
using OrderManagement.Api.Handlers;

namespace OrderManagement.UnitTests.TestDoubles;

public class InMemoryProductRepository : IProductRepository
{
    public readonly List<Product> Products = new();
    
    public Task<Product?> GetProductByIdAsync(int id)
    {
        return Task.FromResult(Products.FirstOrDefault(p => p.Id == id));
    }
    
    public Task UpdateProductAsync(Product product)
    {
        var existingProduct = Products.FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct != null)
        {
            Products.Remove(existingProduct);
            Products.Add(product);
        }
        return Task.CompletedTask;
    }
}