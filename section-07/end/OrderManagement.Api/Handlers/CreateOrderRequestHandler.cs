using OrderManagement.Api.Core;
using OrderManagement.Api.Domain;
using OrderManagement.Api.Models;

namespace OrderManagement.Api.Handlers;

public class CreateOrderRequestHandler : ICreateOrderRequestHandler
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly TimeProvider _timeProvider;
    private readonly IOrderRepository _orderRepository;
    private readonly IShippingService _shippingService;

    public CreateOrderRequestHandler(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        TimeProvider timeProvider, IShippingService shippingService)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _timeProvider = timeProvider;
        _shippingService = shippingService;
    }

    public async Task<Result<OrderDto>> HandleAsync(CreateOrderRequest request)
    {
        if (request.Items == null || !request.Items.Any())
            return Result<OrderDto>.Failure(
                new Error(
                    ErrorCodes.InvalidOrder,
                    "Order must have at least one item"));

        if (_timeProvider.GetUtcNow().Hour >= 14)
            return Result<OrderDto>.Failure(new Error(ErrorCodes.InvalidOrder,
                "Orders placed after 2 PM will be processed tomorrow"));

        var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId);
        if (customer == null)
            return Result<OrderDto>.Failure(new Error(ErrorCodes.NotFound, "Customer not found"));

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetProductByIdAsync(item.ProductId);
            if (product == null)
                return Result<OrderDto>.Failure(new Error(ErrorCodes.InvalidOrder,
                    $"Product {item.ProductId} not found"));

            if (product.StockQuantity < item.Quantity)
                return Result<OrderDto>.Failure(new Error(ErrorCodes.InvalidOrder,
                    $"Insufficient stock for product {product.Name}"));

            product.StockQuantity -= item.Quantity;
            await _productRepository.UpdateProductAsync(product);
        }

        var order = new Order
        {
            CustomerId = request.CustomerId,
            OrderDate = _timeProvider.GetUtcNow().UtcDateTime,
            Status = OrderStatus.Pending,
            Items = new List<OrderItem>(),
            ShippingAddress = request.ShippingAddress
        };

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetProductByIdAsync(item.ProductId);
            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        order = await _orderRepository.AddOrderAsync(order);

        var shippingResponse = await _shippingService.EstimateAsync(order, request.ShippingAddress );

        if (shippingResponse.IsSuccess)
        {
            order.EstimatedDeliveryDate = shippingResponse.Value!.EstimatedDeliveryDate;
            await _orderRepository.UpdateOrderAsync(order);
        }

        return Result<OrderDto>.Success(new OrderDto(order));
    }
}