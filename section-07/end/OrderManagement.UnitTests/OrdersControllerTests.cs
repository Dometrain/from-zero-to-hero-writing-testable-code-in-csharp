using OrderManagement.Api.Controllers;
using OrderManagement.Api.Handlers;
using OrderManagement.Api.Models;
using OrderManagement.Api.Core;
using OrderManagement.Api.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Api;
using OrderManagement.Api.Data;

namespace OrderManagement.UnitTests;

public class SuccessStub : ICreateOrderRequestHandler
{
    public Task<Result<OrderDto>> HandleAsync(CreateOrderRequest request)
    {
        var order = new Order { Id = 1 };
        var orderDto = new OrderDto(order);
        return Task.FromResult(Result<OrderDto>.Success(orderDto));
    }
}

public class NotFoundStub : ICreateOrderRequestHandler
{
    public Task<Result<OrderDto>> HandleAsync(CreateOrderRequest request)
    {
        return Task.FromResult(Result<OrderDto>.Failure(
            new Error(ErrorCodes.NotFound, "Customer not found")));
    }
}

public class BadRequestStub : ICreateOrderRequestHandler
{
    public Task<Result<OrderDto>> HandleAsync(CreateOrderRequest request)
    {
        return Task.FromResult(Result<OrderDto>.Failure(
            new Error(ErrorCodes.InvalidOrder, "Invalid order")));
    }
}

public class InternalServerErrorStub : ICreateOrderRequestHandler
{
    public Task<Result<OrderDto>> HandleAsync(CreateOrderRequest request)
    {
        return Task.FromResult(Result<OrderDto>.Failure(
            new Error("UNKNOWN", "Unexpected error")));
    }
}

public class ExceptionStub : ICreateOrderRequestHandler
{
    public Task<Result<OrderDto>> HandleAsync(CreateOrderRequest request)
    {
        throw new Exception("Test exception");
    }
}

internal class FakeUserContext : IUserContext
{
    public bool IsAuthenticatedValue { get; set; }
    public string UserIdValue { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
        
    public bool IsAuthenticated => IsAuthenticatedValue;
    public string UserId => UserIdValue;
        
    public bool IsInRole(string role)
    {
        return Roles.Contains(role);
    }
}


public class OrdersControllerTests
{

    [Fact]
    public async Task CreateOrder_WhenSuccessful_ReturnsCreated()
    {
        // Arrange
        var request = new CreateOrderRequest();
        var controller = new OrdersController(null!, new SuccessStub(), new FakeUserContext());

        // Act
        var result = await controller.CreateOrder(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(nameof(controller.GetOrder), createdResult.ActionName);
    }

    [Fact]
    public async Task CreateOrder_WhenCustomerNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new CreateOrderRequest();
        var controller = new OrdersController(null!, new NotFoundStub(), new FakeUserContext());

        // Act
        var result = await controller.CreateOrder(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("Customer not found", notFoundResult.Value);
    }

    [Fact]
    public async Task CreateOrder_WhenInvalidOrder_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateOrderRequest();
        var controller = new OrdersController(null!, new BadRequestStub(), new FakeUserContext());

        // Act
        var result = await controller.CreateOrder(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Invalid order", badRequestResult.Value);
    }

    [Fact]
    public async Task CreateOrder_WhenUnexpectedError_ReturnsInternalServerError()
    {
        // Arrange
        var request = new CreateOrderRequest();
        var controller = new OrdersController(null!, new InternalServerErrorStub(), new FakeUserContext());

        // Act
        var result = await controller.CreateOrder(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Unexpected error", statusCodeResult.Value);
    }

    [Fact]
    public async Task CreateOrder_WhenExceptionOccurs_ReturnsInternalServerError()
    {
        // Arrange
        var request = new CreateOrderRequest();
        var controller = new OrdersController(null!, new ExceptionStub(), new FakeUserContext());

        // Act
        var result = await controller.CreateOrder(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("An error occurred while processing your order", statusCodeResult.Value);
    }

    [Fact]
    public async Task CancelOrder_WhenOrderExists_ReturnsNoContent()
    {
        // Arrange
        const int orderId = 1;
        const int customerId = 1;
        
        // Create in-memory database
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: "CancelOrder_WhenOrderExists_ReturnsOk")
            .Options;
            
        // Set up the database with a test order
        await using (var context = new OrderDbContext(options))
        {
            
            context.Orders.Add(new Order 
            { 
                Id = orderId, 
                Status = OrderStatus.Pending,
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }
        
        // Use a separate instance of the context for the controller
        await using (var context = new OrderDbContext(options))
        {
            var userContext = new FakeUserContext();
            userContext.UserIdValue = customerId.ToString();
            userContext.Roles.Add("Admin");
            var controller = new OrdersController(context, null!, userContext);

            // Act
            var result = await controller.CancelOrder(orderId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
            
            // Verify the order was updated with Cancelled status
            var updatedOrder = await context.Orders.FindAsync(orderId);
            Assert.Equal(OrderStatus.Cancelled, updatedOrder!.Status);
        }
    }
}