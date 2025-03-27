using OrderManagement.CompositionOfNarrowComponents;

namespace OrderManagement.Tests.CompositionOfNarrowComponents;

public class OrderCheckoutServiceTests
{
    private readonly StubInventoryValidator _inventoryValidator;
    private readonly StubPriceCalculator _priceCalculator;
    private readonly SpyPaymentProcessor _paymentProcessor;
    private readonly SpyOrderCreator _orderCreator;
    private readonly OrderCheckoutService _checkoutService;

    public OrderCheckoutServiceTests()
    {
        _inventoryValidator = new StubInventoryValidator();
        _priceCalculator = new StubPriceCalculator(100m);
        _paymentProcessor = new SpyPaymentProcessor();
        _orderCreator = new SpyOrderCreator(123);

        _checkoutService = new OrderCheckoutService(
            _inventoryValidator,
            _priceCalculator,
            _paymentProcessor,
            _orderCreator
        );
    }

    [Fact]
    public void Checkout_WhenSuccessful_ReturnsSuccessResult()
    {
        // Arrange
        var cart = new ShoppingCart 
        { 
            CustomerId = 1,
            Items = [new CartItem { ProductId = 1, Quantity = 2 }]
        };
        var paymentInfo = new PaymentInfo();
        const int expectedOrderId = 123;
        const decimal expectedTotal = 100m;

        _inventoryValidator.SetHasInventory(true);

        // Act
        var result = _checkoutService.Checkout(cart, paymentInfo);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(expectedOrderId, result.OrderId);
        Assert.Equal(1, _paymentProcessor.ProcessPaymentCallCount);
        Assert.Equal(paymentInfo, _paymentProcessor.LastPaymentInfo);
        Assert.Equal(expectedTotal, _paymentProcessor.LastAmount);
        Assert.Equal(cart, _orderCreator.LastCart);
        Assert.Equal(expectedTotal, _orderCreator.LastTotalPrice);
    }

    [Fact]
    public void Checkout_WhenInsufficientInventory_ReturnsFailureResult()
    {
        // Arrange
        var cart = new ShoppingCart 
        { 
            Items = [new CartItem { ProductId = 1, Quantity = 2 }]
        };
        var paymentInfo = new PaymentInfo();

        _inventoryValidator.SetHasInventory(false);

        // Act
        var result = _checkoutService.Checkout(cart, paymentInfo);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Not enough inventory", result.Error);
        Assert.Equal(0, _paymentProcessor.ProcessPaymentCallCount);
        Assert.Null(_orderCreator.LastCart);
    }

    [Fact]
    public void Checkout_WhenPaymentFails_ReturnsFailureResult()
    {
        // Arrange
        var cart = new ShoppingCart 
        { 
            Items = [new CartItem { ProductId = 1, Quantity = 2 }]
        };
        var paymentInfo = new PaymentInfo();
        const string expectedError = "Payment failed";
        var paymentProcessor = new SpyPaymentProcessor(new Exception(expectedError));

        _inventoryValidator.SetHasInventory(true);

        var service = new OrderCheckoutService(
            _inventoryValidator,
            _priceCalculator,
            paymentProcessor,
            _orderCreator
        );

        // Act
        var result = service.Checkout(cart, paymentInfo);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(expectedError, result.Error);
        Assert.Equal(1, paymentProcessor.ProcessPaymentCallCount);
        Assert.Null(_orderCreator.LastCart);
    }
} 