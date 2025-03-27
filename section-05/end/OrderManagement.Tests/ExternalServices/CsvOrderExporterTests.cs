using OrderManagement.ExternalServices;
using System.IO;

namespace OrderManagement.Tests.ExternalServices;

public class CsvOrderExporterTests
{
    private readonly string _testOutputPath = Path.Combine(Path.GetTempPath(), "test_orders", "order.csv");
    private readonly CsvOrderExporter _exporter;

    public CsvOrderExporterTests()
    {
        _exporter = new CsvOrderExporter();
        // Ensure test directory is clean before each test
        if (Directory.Exists(Path.GetDirectoryName(_testOutputPath)))
        {
            Directory.Delete(Path.GetDirectoryName(_testOutputPath), true);
        }
    }

    [Fact]
    public void ExportOrder_WithValidOrder_CreatesCorrectCsvFile()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            OrderDate = new DateTime(2024, 3, 20),
            TotalAmount = 150m,
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = 1,
                    ProductName = "Test Product",
                    Quantity = 2,
                    UnitPrice = 75m
                }
            }
        };

        // Act
        _exporter.ExportOrder(order, _testOutputPath);

        // Assert
        Assert.True(File.Exists(_testOutputPath));
        var fileContent = File.ReadAllLines(_testOutputPath);
        
        Assert.Equal("OrderId,CustomerName,OrderDate,TotalAmount", fileContent[0]);
        Assert.Equal("1,John Doe,2024-03-20,150", fileContent[1]);
        Assert.Equal(string.Empty, fileContent[2]);
        Assert.Equal("ItemId,ProductName,Quantity,UnitPrice,LineTotal", fileContent[3]);
        Assert.Equal("1,Test Product,2,75,150", fileContent[4]);
    }

    [Fact]
    public void ExportOrder_WithMultipleItems_CreatesCorrectCsvFile()
    {
        // Arrange
        var order = new Order
        {
            Id = 2,
            CustomerName = "Jane Smith",
            OrderDate = new DateTime(2024, 3, 20),
            TotalAmount = 250m,
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    Id = 1,
                    ProductName = "Product 1",
                    Quantity = 2,
                    UnitPrice = 50m
                },
                new OrderItem
                {
                    Id = 2,
                    ProductName = "Product 2",
                    Quantity = 3,
                    UnitPrice = 50m
                }
            }
        };

        // Act
        _exporter.ExportOrder(order, _testOutputPath);

        // Assert
        Assert.True(File.Exists(_testOutputPath));
        var fileContent = File.ReadAllLines(_testOutputPath);
        
        Assert.Equal(6, fileContent.Length);
        Assert.Equal("2,Jane Smith,2024-03-20,250", fileContent[1]);
        Assert.Equal("1,Product 1,2,50,100", fileContent[4]);
        Assert.Equal("2,Product 2,3,50,150", fileContent[5]);
    }

    [Fact]
    public void ExportOrder_CreatesDirectoryIfNotExists()
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

        var deepPath = Path.Combine(Path.GetTempPath(), "test_orders", "subfolder", "deep", "order.csv");

        // Act
        _exporter.ExportOrder(order, deepPath);

        // Assert
        Assert.True(Directory.Exists(Path.GetDirectoryName(deepPath)));
        Assert.True(File.Exists(deepPath));
    }
} 