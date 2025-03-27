namespace Exercises.Tests;

public class OrderServiceBuilder
{
    private OrderService _service;
    private Customer _customer;
    private Product _product;
    private int _stockQuantity = 10;

    public OrderServiceBuilder(string validStoreLocation)
    {
        _service = new OrderService(validStoreLocation);
    }

    public OrderServiceBuilder WithProduct(string id, int stockQuantity = 10)
    {
        _stockQuantity = stockQuantity;
        _product = new Product
            { Id = id, Name = "Test Product", Price = 10.0m, StockQuantity = _stockQuantity };
        _service.AddProductToInventory(_product);
        return this;
    }

    public OrderServiceBuilder WithCustomer(string id)
    {
        _customer = new Customer { Id = id, Name = "Test Customer" };
        return this;
    }


    public (OrderService service, Customer customer, Product product) Build()
    {
        return (_service, _customer, _product);
    }
}