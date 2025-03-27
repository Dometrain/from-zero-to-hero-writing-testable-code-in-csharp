using Exercises.Tests.TestDoubles;

namespace Exercises.Tests;

public class OrderServiceTests
{
    [Fact]
    public void CreateOrder_GeneratesPredictableReferenceNumber()
    {
        // Arrange
        var dateProvider = new FakeDateTimeProvider
        {
            NowValue = new DateTime(2023, 6, 1, 12, 30, 0)
        };

        var randomGenerator = new FakeRandomNumberGenerator
        {
            ValueToReturn = 500
        };

        var configProvider = new FakeConfigProvider
        {
            PrefixToReturn = "TEST",
            SequenceToReturn = 2000
        };

        var service = CreateOrderService(
            dateTimeProvider: dateProvider,
            randomNumberGenerator: randomGenerator,
            configProvider: configProvider,
            shippingCalculator: null);

        var customer = new Customer()
        {
            Membership = new Membership()
            {
                Level = MembershipLevel.Standard
            }
        };
        var address = new Address();
        var orderLines = new List<OrderLine>
        {
            new OrderLine { Quantity = 1, UnitPrice = 10.0m }
        };

        // Act
        var order = service.CreateOrder(customer, address, orderLines);

        // Assert
        // Reference should be PREFIX-TIMESTAMP-SEQUENCE
        Assert.Equal("TEST-202306011230-2500", order.ReferenceNumber);
        Assert.Equal(new DateTime(2023, 6, 1, 12, 30, 0), order.CreatedAt);
    }

    [Fact]
    public async Task NotifyFulfillmentTeam_CallsApiClientWithCorrectOrderId()
    {
        // Arrange
        var fulfillmentClient = new FakeFulfillmentApiClient
        {
            SuccessResult = true
        };

        var service = CreateOrderService(fulfillmentClient: fulfillmentClient);

        var order = new Order { Id = 123 };

        // Act
        var result = await service.NotifyFulfillmentTeam(order);

        // Assert
        Assert.True(result);
        Assert.Equal(123, fulfillmentClient.LastOrderId);
    }
    
    
    [Fact]
    public void ProcessOrder_ValidPendingOrder_RecordsCorrectMetrics()
    {
        // Arrange
        var testRecorder = new FakeMetricsRecorder();
        var service = CreateOrderService(metricsRecorder: testRecorder);

        var order = new Order
        {
            Id = 1,
            Status = OrderStatus.Pending
        };

        // Act
        var result = service.ProcessOrder(order);

        // Assert
        Assert.True(result);
        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.Contains("OrderProcessed:1", testRecorder.RecordedMetrics);
        Assert.Contains("Counter:orders_processed", testRecorder.RecordedMetrics);
    }

    [Fact]
    public void ProcessOrder_CancelledOrder_RecordsCorrectMetricsAndReturnsFalse()
    {
        // Arrange
        var testRecorder = new FakeMetricsRecorder();
        var service = CreateOrderService(metricsRecorder: testRecorder);

        var order = new Order
        {
            Id = 2,
            Status = OrderStatus.Cancelled
        };

        // Act
        var result = service.ProcessOrder(order);

        // Assert
        Assert.False(result);
        Assert.Equal(OrderStatus.Cancelled, order.Status); // Status unchanged
        Assert.Contains("OrderCancelled:2:Processing attempted on cancelled order",
            testRecorder.RecordedMetrics);
    }
    
    private static OrderService CreateOrderService(
        IMetricsRecorder metricsRecorder = null,
        ShippingCostCalculator? shippingCalculator = null,
        IOrderPrinter orderPrinter = null,
        IConfigProvider configProvider = null,
        IDateTimeProvider dateTimeProvider = null,
        IRandomNumberGenerator randomNumberGenerator = null,
        IFulfillmentApiClient fulfillmentClient = null)
    {
        // Create default test implementations if not provided
        metricsRecorder ??= new FakeMetricsRecorder();

        // If a shipping calculator wasn't provided, create one with test dependencies
        if (shippingCalculator == null)
        {
            var rateProvider = new FakeShippingRateProvider();
            var discountProvider = new FakeMembershipDiscountProvider();
            shippingCalculator = new ShippingCostCalculator(rateProvider, discountProvider);
        }

        orderPrinter ??= new StringOrderPrinter();
        configProvider ??= new FakeConfigProvider();
        dateTimeProvider ??= new FakeDateTimeProvider();
        randomNumberGenerator ??= new FakeRandomNumberGenerator();
        fulfillmentClient ??= new FakeFulfillmentApiClient();

        return new OrderService(
            metricsRecorder,
            shippingCalculator,
            orderPrinter,
            configProvider,
            dateTimeProvider,
            randomNumberGenerator,
            fulfillmentClient);
    }
}