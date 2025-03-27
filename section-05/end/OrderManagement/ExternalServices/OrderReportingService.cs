namespace OrderManagement.ExternalServices;

public class OrderReportingService
{
    private readonly IOrderExporter _exporter;

    public OrderReportingService(IOrderExporter exporter)
    {
        _exporter = exporter ?? throw new ArgumentNullException(nameof(exporter));
    }

    public void GenerateOrderReport(Order order, string outputPath)
    {
        if (order == null) throw new ArgumentNullException(nameof(order));
        if (outputPath == null) throw new ArgumentNullException(nameof(outputPath));

        _exporter.ExportOrder(order, outputPath);

        Console.WriteLine($"Order report generated: {outputPath}");
    }
}