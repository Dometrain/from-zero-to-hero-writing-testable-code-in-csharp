namespace OrderManagement.ExternalServices;

public interface IOrderExporter
{
    void ExportOrder(Order order, string filePath);
}
public class CsvOrderExporter : IOrderExporter
{
    public void ExportOrder(Order order, string filePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        using (var streamWriter = new StreamWriter(filePath))
        {
            // Write header row
            streamWriter.WriteLine("OrderId,CustomerName,OrderDate,TotalAmount");

            // Write order data
            streamWriter.WriteLine(
                $"{order.Id},{order.CustomerName},{order.OrderDate:yyyy-MM-dd},{order.TotalAmount}");

            // Write line items header
            streamWriter.WriteLine("\nItemId,ProductName,Quantity,UnitPrice,LineTotal");

            // Write item details
            foreach (var item in order.Items)
            {
                streamWriter.WriteLine(
                    $"{item.Id},{item.ProductName},{item.Quantity},{item.UnitPrice},{item.Quantity * item.UnitPrice}");
            }
        }
    }
}