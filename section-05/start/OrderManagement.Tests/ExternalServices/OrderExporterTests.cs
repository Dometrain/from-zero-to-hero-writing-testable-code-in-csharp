using OrderManagement.ExternalServices;

namespace OrderManagement.Tests.ExternalServices;

public class OrderExporterTests
{
    [Fact]
    public void ExportOrdersToCsv_WritesCorrectData()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            CustomerName = "Gui Ferreira",
            OrderDate = new DateTime(2023, 1, 15),
            TotalAmount = 125m,
            Items =
                [
                    new OrderItem()
                    {
                        Id = 1,
                        ProductName = "Apple",
                        Quantity = 5,
                        UnitPrice = 10m,
                    }
                ]

        };

        var reportingService = new OrderReportingService();
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            reportingService.GenerateOrderReport(order, tempFile);

            // Assert
            var fileContents = File.ReadAllText(tempFile);
            Assert.Contains("OrderId,CustomerName,OrderDate,TotalAmount", fileContents);
            Assert.Contains("ItemId,ProductName,Quantity,UnitPrice,LineTotal", fileContents);
            Assert.Contains("1,Apple,5,10,50", fileContents);

            // PROBLEM: This test has several issues:
            // 1. It writes to the actual file system during testing
            // 2. If file system has permission issues, test fails
            // 3. Test is slower due to I/O operations
            // 4. Test cleanup is required (file deletion)
            // 5. Tests may fail when run in parallel if they use same file
        }
        finally
        {
            // Clean up
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}