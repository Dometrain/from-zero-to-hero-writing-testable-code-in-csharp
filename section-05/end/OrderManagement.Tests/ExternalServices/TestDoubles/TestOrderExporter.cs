using OrderManagement.ExternalServices;

namespace OrderManagement.Tests.ExternalServices.TestDoubles;

public class TestOrderExporter : IOrderExporter
{
    private Order _lastExportedOrder;
    private string _lastExportPath;
    private int _exportCallCount;

    public void ExportOrder(Order order, string filePath)
    {
        _lastExportedOrder = order;
        _lastExportPath = filePath;
        _exportCallCount++;
    }

    public Order GetLastExportedOrder() => _lastExportedOrder;
    public string GetLastExportPath() => _lastExportPath;
    public int GetExportCallCount() => _exportCallCount;
} 