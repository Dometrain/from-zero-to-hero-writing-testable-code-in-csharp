namespace Exercises.Tests;

public class OrderServiceTests
{
    private const string ValidStoreLocation = "TestLocation";
    private const string NonExistentOrderId = "non-existent-id";
    private const string CustomerId = "customer-1";
    private const string ProductId1 = "product-1";
    private const string ProductId2 = "product-2";
    private const string NonExistentProductId = "non-existent-product";
    private const decimal ProductPrice = 100.0m;

    [Fact]
    public void Should_create_instance_when_valid_store_location_provided()
    {
        // Arrange & Act
        var service = new OrderService(ValidStoreLocation);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public void Should_throw_argument_null_exception_when_null_store_location_provided()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new OrderService(null!));
        Assert.Equal("storeLocation", exception.ParamName);
    }

    [Fact]
    public void Should_return_failure_when_processing_order_with_empty_id()
    {
        // Arrange
        var service = CreateOrderService();

        // Act
        var result = service.ProcessOrder("", OrderStatus.Processing);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Order ID cannot be empty", result.Error?.Message);
    }

    [Fact]
    public void Should_return_failure_when_processing_non_existent_order()
    {
        // Arrange
        var service = CreateOrderService();

        // Act
        var result = service.ProcessOrder(NonExistentOrderId, OrderStatus.Processing);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Order with ID {NonExistentOrderId} not found", result.Error?.Message);
    }

    [Fact]
    public void Should_update_order_status_when_processing_existing_order()
    {
        // Arrange
        var builder = new OrderServiceBuilder(ValidStoreLocation);
        var (service, customer, product) = builder
            .WithCustomer(CustomerId)
            .WithProduct(ProductId1)
            .Build();
        var orderItems = new List<OrderItem> { new() { ProductId = product.Id, Quantity = 1 } };
        var createResult = service.CreateOrder(customer, orderItems);
        Assert.True(createResult.IsSuccess);

        var orderId = createResult.Value!.Id;

        // Act
        var result = service.ProcessOrder(orderId, OrderStatus.Shipped);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.Shipped, result.Value!.Status);
    }

    [Fact]
    public void Should_return_failure_when_creating_order_with_null_customer()
    {
        // Arrange
        var service = CreateOrderService();
        var items = new List<OrderItem> { new() { ProductId = ProductId1, Quantity = 1 } };

        // Act
        var result = service.CreateOrder(null!, items);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Customer cannot be null", result.Error?.Message);
    }

    [Fact]
    public void Should_return_failure_when_creating_order_with_empty_items()
    {
        // Arrange
        var service = CreateOrderService();
        var customer = CreateCustomer();

        // Act
        var result = service.CreateOrder(customer, new List<OrderItem>());

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Order must contain at least one item", result.Error?.Message);
    }

    [Fact]
    public void Should_return_failure_when_creating_order_with_non_existent_product()
    {
        // Arrange
        var service = CreateOrderService();
        var customer = CreateCustomer();
        var items = new List<OrderItem> { new() { ProductId = NonExistentProductId, Quantity = 1 } };

        // Act
        var result = service.CreateOrder(customer, items);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Product with ID {NonExistentProductId} not found", result.Error?.Message);
    }

    [Fact]
    public void Should_return_failure_when_creating_order_with_insufficient_stock()
    {
        // Arrange
        var builder = new OrderServiceBuilder(ValidStoreLocation);
        var (service, customer, product) = builder
            .WithCustomer(CustomerId)
            .WithProduct(ProductId1, stockQuantity: 5).Build();
        var items = new List<OrderItem> { new() { ProductId = product.Id, Quantity = 10 } };

        // Act
        var result = service.CreateOrder(customer, items);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Insufficient stock for product {product.Id}", result.Error?.Message);
    }

    [Fact]
    public void Should_create_order_with_pending_status_when_valid_inputs_provided_without_priority()
    {
        // Arrange
        var builder = new OrderServiceBuilder(ValidStoreLocation);
        var (service, customer, product) = builder
            .WithCustomer(CustomerId)
            .WithProduct(ProductId1, stockQuantity: 10).Build();
        var items = new List<OrderItem> { new() { ProductId = product.Id, Quantity = 5 } };

        // Act
        var result = service.CreateOrder(customer, items);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(customer.Id, result.Value!.CustomerId);
        Assert.Equal(OrderStatus.Pending, result.Value!.Status);
        Assert.Equal(5, product.StockQuantity); // Verify stock was reduced
    }

    [Fact]
    public void Should_create_order_with_processing_status_when_valid_inputs_provided_with_priority()
    {
        // Arrange
        var builder = new OrderServiceBuilder(ValidStoreLocation);
        var (service, customer, product) = builder
            .WithCustomer(CustomerId)
            .WithProduct(ProductId1,stockQuantity: 10).Build();
        var items = new List<OrderItem> { new() { ProductId = product.Id, Quantity = 5 } };

        // Act
        var result = service.CreateOrder(customer, items, true);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(customer.Id, result.Value!.CustomerId);
        Assert.Equal(OrderStatus.Processing, result.Value!.Status);
        Assert.Equal(5, product.StockQuantity); // Verify stock was reduced
    }

    [Fact]
    public void Should_reduce_product_stock_when_creating_valid_order()
    {
        // Arrange
        var service = CreateOrderService();
        var customer = CreateCustomer();
        var product1 = CreateProduct(ProductId1, "Test Product 1", 10.0m, 10);
        var product2 = CreateProduct(ProductId2, "Test Product 2", 20.0m, 20);
        service.AddProductToInventory(product1);
        service.AddProductToInventory(product2);

        var items = new List<OrderItem>
        {
            new() { ProductId = product1.Id, Quantity = 10 },
            new() { ProductId = product2.Id, Quantity = 5 }
        };

        // Act
        var result = service.CreateOrder(customer, items);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, product1.StockQuantity);
        Assert.Equal(15, product2.StockQuantity);
    }

    [Fact]
    public void Should_return_failure_when_applying_discount_with_empty_order_id()
    {
        // Arrange
        var service = CreateOrderService();

        // Act
        var result = service.ApplyDiscount("", 10);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Order ID cannot be empty", result.Error?.Message);
    }

    [Fact]
    public void Should_return_failure_when_applying_discount_with_negative_percentage()
    {
        // Arrange
        var service = CreateOrderService();

        // Act
        var result = service.ApplyDiscount("order-1", -10);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Discount percentage must be between 0 and 100", result.Error?.Message);
    }

    [Fact]
    public void Should_return_failure_when_applying_discount_with_percentage_over_100()
    {
        // Arrange
        var service = CreateOrderService();

        // Act
        var result = service.ApplyDiscount("order-1", 110);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Discount percentage must be between 0 and 100", result.Error?.Message);
    }

    [Fact]
    public void Should_return_failure_when_applying_discount_to_non_existent_order()
    {
        // Arrange
        var service = CreateOrderService();

        // Act
        var result = service.ApplyDiscount(NonExistentOrderId, 10);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"Order with ID {NonExistentOrderId} not found", result.Error?.Message);
    }

    [Theory]
    [InlineData(200, 20, 80)]
    [InlineData(200, 50, 200)]
    [InlineData(150, 100, 300)]
    public void Should_calculate_correct_discount_amount_when_applying_valid_discount(decimal productPrice, decimal discountPercentage, decimal expectedDiscountAmount)
    {
        // Arrange
        var (order, service) = CreateServiceWithOrder(productPrice: productPrice, quantity: 2);

        // Act
        var result = service.ApplyDiscount(order.Id, discountPercentage);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedDiscountAmount, result.Value!.DiscountAmount);
    }

    [Fact]
    public void Should_return_zero_discount_when_applying_zero_percent_discount()
    {
        // Arrange
        var (order, service) = CreateServiceWithOrder(productPrice: ProductPrice, quantity: 2);

        // Act
        var result = service.ApplyDiscount(order.Id, 0);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0.0m, result.Value!.DiscountAmount);
    }

    [Fact]
    public void Should_return_failure_when_getting_customer_with_empty_id()
    {
        // Arrange
        var service = CreateOrderService();

        // Act
        var result = service.GetCustomer("");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Customer ID cannot be empty", result.Error?.Message);
    }

    [Fact]
    public void Should_return_customer_when_getting_customer_with_valid_id()
    {
        // Arrange
        var service = CreateOrderService();

        // Act
        var result = service.GetCustomer(CustomerId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(CustomerId, result.Value!.Id);
        Assert.Equal("Test Customer", result.Value!.Name);
    }

    [Fact]
    public void Should_return_true_when_store_location_is_warehouse()
    {
        // Arrange
        var service = new OrderService("Warehouse");

        // Act
        var result = service.IsExpressShippingAvailable();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Should_return_true_when_store_location_is_mall()
    {
        // Arrange
        var service = new OrderService("Mall");

        // Act
        var result = service.IsExpressShippingAvailable();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Should_return_false_when_store_location_is_not_warehouse_or_mall()
    {
        // Arrange
        var service = new OrderService("LocalStore");

        // Act
        var result = service.IsExpressShippingAvailable();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Should_throw_argument_null_exception_when_adding_null_product()
    {
        // Arrange
        var service = CreateOrderService();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => service.AddProductToInventory(null!));
        Assert.Equal("product", exception.ParamName);
    }

    [Fact]
    public void Should_add_product_to_inventory_when_valid_product_provided()
    {
        // Arrange
        var service = CreateOrderService();
        var product = CreateProduct();

        // Act
        service.AddProductToInventory(product);

        // Assert - We can verify the product was added by creating an order with this product
        var customer = CreateCustomer();
        var items = new List<OrderItem> { new() { ProductId = product.Id, Quantity = 1 } };
        var result = service.CreateOrder(customer, items);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Should_handle_multiple_errors_when_creating_order_with_multiple_invalid_products()
    {
        // Arrange
        var service = CreateOrderService();
        var customer = CreateCustomer();
        var product = CreateProduct(stockQuantity: 2);
        service.AddProductToInventory(product);

        var items = new List<OrderItem>
        {
            new() { ProductId = "non-existent-product-1", Quantity = 1 },
            new() { ProductId = "non-existent-product-2", Quantity = 1 },
            new() { ProductId = product.Id, Quantity = 5 } // Insufficient stock
        };

        // Act
        var result = service.CreateOrder(customer, items);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Product with ID non-existent-product-1 not found", result.Error?.Message);
        Assert.Contains("Product with ID non-existent-product-2 not found", result.Error?.Message);
        Assert.Contains($"Insufficient stock for product {product.Id}", result.Error?.Message);
    }


    private static OrderService CreateOrderService() => new(ValidStoreLocation);

    private (Order order, OrderService service) CreateServiceWithOrder(decimal productPrice, int quantity)
    {
        var service = CreateOrderService();
        var customer = CreateCustomer();
        var product = CreateProduct(ProductId1, "Test Product", productPrice, 10!);
        service.AddProductToInventory(product);

        var items = new List<OrderItem> { new() { ProductId = product.Id, Quantity = quantity } };
        var orderResult = service.CreateOrder(customer, items);
        return (orderResult.Value!, service);
    }

    private static Customer CreateCustomer() => new() { Id = CustomerId, Name = "Test Customer" };

    private static Product CreateProduct(string id = ProductId1, string name = "Test Product", decimal price = 10.0m,
        int stockQuantity = 10) =>
        new() { Id = id, Name = name, Price = price, StockQuantity = stockQuantity };
}