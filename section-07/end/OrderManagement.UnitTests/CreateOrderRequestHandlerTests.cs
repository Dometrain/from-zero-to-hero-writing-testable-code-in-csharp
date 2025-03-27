using Microsoft.Extensions.Time.Testing;
using OrderManagement.Api.Core;
using OrderManagement.Api.Domain;
using OrderManagement.Api.Handlers;
using OrderManagement.Api.Models;
using OrderManagement.UnitTests.TestDoubles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OrderManagement.UnitTests;

public class CreateOrderRequestHandlerTests
{
    private readonly InMemoryCustomerRepository _customerRepository;
    private readonly InMemoryProductRepository _productRepository;
    private readonly InMemoryOrderRepository _orderRepository;
    private readonly CreateOrderRequestHandler _handler;
    private readonly FakeTimeProvider _timeProvider;
    private readonly StubShippingService _shippingService;

    public CreateOrderRequestHandlerTests()
    {
        _customerRepository = new InMemoryCustomerRepository();
        _productRepository = new InMemoryProductRepository();
        _orderRepository = new InMemoryOrderRepository();
        _timeProvider = new FakeTimeProvider();
        _shippingService = new StubShippingService();

        _handler = new CreateOrderRequestHandler(
            _orderRepository,
            _customerRepository,
            _productRepository,
            _timeProvider,
            _shippingService);
    }

    [Fact]
    public async Task HandleAsync_WhenCustomerNotFound_ReturnsNotFoundError()
    {
        // Arrange
        var request = new CreateOrderRequest
        {
            CustomerId = 999, // Non-existent customer ID
            Items = new List<OrderItemRequest>
            {
                new OrderItemRequest { ProductId = 1, Quantity = 1 }
            }
        };

        // Act
        var result = await _handler.HandleAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCodes.NotFound, result.Error!.Value.ErrorCode);
        Assert.Equal("Customer not found", result.Error!.Value.Message);
    }


    [Fact]
    public async Task HandleAsync_WhenAfter2PM_ReturnsInvalidOrderError()
    {
        // Arrange
        _customerRepository.Customers.Add(new Customer { Id = 1 });
        var request = new CreateOrderRequest
        {
            CustomerId = 1,
            Items = new List<OrderItemRequest>
            {
                new OrderItemRequest { ProductId = 1, Quantity = 1 }
            }
        };
        _timeProvider.SetUtcNow(new DateTimeOffset(2020, 1, 1, 
            14, 1, 0, 
            TimeSpan.Zero));

        // Act
        var result = await _handler.HandleAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCodes.InvalidOrder, result.Error!.Value.ErrorCode);
        Assert.Equal("Orders placed after 2 PM will be processed tomorrow", result.Error!.Value.Message);
        
    }

    [Fact]
    public async Task HandleAsync_WithValidRequest_ReturnsSuccessfulOrder()
    {
        // Arrange
        const int customerId = 1;
        const int productId = 1;
        const int quantity = 2;
        const decimal productPrice = 10.0m;
        var estimatedDeliveryDate = DateTime.UtcNow.AddDays(3);

        _customerRepository.Customers.Add(new Customer { Id = customerId });
        _productRepository.Products.Add(new Product 
        { 
            Id = productId, 
            Price = productPrice, 
            StockQuantity = 10,
            Name = "Test Product"
        });

        var shippingAddress = new Address
        {
            Street = "123 Test St",
            City = "Test City",
            State = "TS"
        };

        var request = new CreateOrderRequest
        {
            CustomerId = customerId,
            Items = new List<OrderItemRequest>
            {
                new OrderItemRequest { ProductId = productId, Quantity = quantity }
            },
            ShippingAddress = shippingAddress
        };

        _timeProvider.SetUtcNow(new DateTimeOffset(2020, 1, 1, 10, 0, 0, TimeSpan.Zero));
        _shippingService.EstimatedDeliveryDate = estimatedDeliveryDate;

        // Act
        var result = await _handler.HandleAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(OrderStatus.Pending, result.Value.Status);
        Assert.Single(result.Value.Items);
        Assert.Equal(productId, result.Value.Items[0].ProductId);
        Assert.Equal(quantity, result.Value.Items[0].Quantity);
        Assert.Equal(productPrice, result.Value.Items[0].UnitPrice);
        Assert.Equal(estimatedDeliveryDate, result.Value.EstimatedDeliveryDate);
        
        // Verify product stock was updated
        var updatedProduct = _productRepository.Products.Find(p => p.Id == productId);
        Assert.Equal(8, updatedProduct!.StockQuantity);
        
        // Verify order was saved
        Assert.Single(_orderRepository.Orders);
    }
}