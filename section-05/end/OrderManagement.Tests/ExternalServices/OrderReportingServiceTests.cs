using OrderManagement.ExternalServices;
using OrderManagement.Tests.ExternalServices.TestDoubles;

namespace OrderManagement.Tests.ExternalServices;

[Collection("OrderReportingService Tests")]
public class OrderReportingServiceTests : IDisposable
{
    private readonly TestOrderExporter _testExporter;
    private readonly OrderReportingService _reportingService;

    public OrderReportingServiceTests()
    {
        _testExporter = new TestOrderExporter();
        _reportingService = new OrderReportingService(_testExporter);
    }

    public void Dispose()
    {
        // Clean up any resources if needed
    }

    [Fact]
    public void GenerateOrderReport_CallsExporterWithCorrectParameters()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            CustomerName = "Test Customer",
            OrderDate = DateTime.Now,
            TotalAmount = 100.00m,
            Items = new List<OrderItem>()
        };
        var outputPath = "test/path/order.csv";

        // Act
        _reportingService.GenerateOrderReport(order, outputPath);

        // Assert
        Assert.Equal(order, _testExporter.GetLastExportedOrder());
        Assert.Equal(outputPath, _testExporter.GetLastExportPath());
        Assert.Equal(1, _testExporter.GetExportCallCount());
    }

    [Fact]
    public void GenerateOrderReport_WithNullOrder_ThrowsArgumentNullException()
    {
        // Arrange
        Order order = null;
        var outputPath = "test/path/order.csv";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => 
            _reportingService.GenerateOrderReport(order, outputPath));
        Assert.Equal("order", exception.ParamName);
        Assert.Equal(0, _testExporter.GetExportCallCount());
    }

    [Fact]
    public void GenerateOrderReport_WithNullOutputPath_ThrowsArgumentNullException()
    {
        // Arrange
        var order = new Order();
        string outputPath = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => 
            _reportingService.GenerateOrderReport(order, outputPath));
        Assert.Equal("outputPath", exception.ParamName);
        Assert.Equal(0, _testExporter.GetExportCallCount());
    }
} 