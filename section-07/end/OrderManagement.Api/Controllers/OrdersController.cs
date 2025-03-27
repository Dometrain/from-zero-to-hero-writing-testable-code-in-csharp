using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using OrderManagement.Api.Data;
using OrderManagement.Api.Domain;
using OrderManagement.Api.Models;
using Microsoft.AspNetCore.Authorization;
using OrderManagement.Api.Core;
using OrderManagement.Api.Handlers;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderDbContext _dbContext;
    private readonly ICreateOrderRequestHandler _createOrderRequestHandler;
    private readonly IUserContext _userContext;

    public OrdersController(OrderDbContext dbContext, ICreateOrderRequestHandler handler, IUserContext userContext)
    {
        _dbContext = dbContext;
        _createOrderRequestHandler = handler;
        _userContext = userContext;
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
            var result = await _createOrderRequestHandler.HandleAsync(request);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetOrder), new { id = result.Value!.Id }, result.Value);

            return result.Error!.Value.ErrorCode switch
            {
                ErrorCodes.NotFound => NotFound(result.Error!.Value.Message),
                ErrorCodes.InvalidOrder => BadRequest(result.Error!.Value.Message),
                _ => StatusCode(500, result.Error!.Value.Message)
            };
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
        
        if (order.CustomerId.ToString() != _userContext.UserId && !_userContext.IsInRole("Admin"))
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