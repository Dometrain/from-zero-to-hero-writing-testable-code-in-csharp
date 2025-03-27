namespace OrderManagement.ExternalServices;

public class OrderReportingService
{
    public void GenerateOrderReport(Order order, string outputPath)
    {
        var exporter = new CsvOrderExporter();

        exporter.ExportOrder(order, outputPath);

        Console.WriteLine($"Order report generated: {outputPath}");
    }
}