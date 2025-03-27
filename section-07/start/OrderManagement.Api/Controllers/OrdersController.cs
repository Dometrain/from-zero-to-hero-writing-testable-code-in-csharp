using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using OrderManagement.Api.Data;
using OrderManagement.Api.Domain;
using OrderManagement.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderDbContext _dbContext;

    public OrdersController(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
        var orders = await _dbContext.Orders
            .Include(o => o.Items)
            .ToListAsync();

        return Ok(orders.Select(o => new OrderDto(o)).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var order = await _dbContext.Orders
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (order.CustomerId.ToString() != userId && !User.IsInRole("Admin"))
            return Forbid();

        return Ok(new OrderDto(order));
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderRequest request)
    {
        try
        {
            if (request.Items == null || !request.Items.Any())
                return BadRequest("Order must have at least one item");

            if (DateTime.UtcNow.Hour >= 14)
                return BadRequest("Orders placed after 2 PM will be processed tomorrow");

            var customer = await _dbContext.Customers.FindAsync(request.CustomerId);
            if (customer == null)
                return NotFound("Customer not found");

            foreach (var item in request.Items)
            {
                var product = await _dbContext.Products.FindAsync(item.ProductId);
                if (product == null)
                    return BadRequest($"Product {item.ProductId} not found");

                if (product.StockQuantity < item.Quantity)
                    return BadRequest($"Insufficient stock for product {product.Name}");

                product.StockQuantity -= item.Quantity;
            }

            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                Items = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = _dbContext.Products.Find(i.ProductId).Price
                }).ToList(),
                ShippingAddress = request.ShippingAddress
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://shipping-api.example.com/")
            };

            var shippingResponse = await httpClient.PostAsJsonAsync("api/shipping/estimate", new
            {
                OrderId = order.Id,
                Address = request.ShippingAddress,
                ItemCount = order.Items.Sum(i => i.Quantity)
            });

            if (shippingResponse.IsSuccessStatusCode)
            {
                var shippingEstimate = await shippingResponse.Content
                    .ReadFromJsonAsync<ShippingEstimateResponse>();
                order.EstimatedDeliveryDate = shippingEstimate.EstimatedDeliveryDate;
                await _dbContext.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, new OrderDto(order));
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your order");
        }
    }

    [HttpPut("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var order = await _dbContext.Orders.FindAsync(id);
        if (order == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (order.CustomerId.ToString() != userId && !User.IsInRole("Admin"))
            return Forbid();

        if (order.Status == OrderStatus.Shipped)
            return BadRequest("Cannot cancel shipped orders");

        if ((DateTime.UtcNow - order.OrderDate).TotalHours > 24)
            return BadRequest("Orders can only be cancelled within 24 hours");

        order.Status = OrderStatus.Cancelled;
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}