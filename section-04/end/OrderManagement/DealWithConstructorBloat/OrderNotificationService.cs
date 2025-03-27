namespace OrderManagement.DealWithConstructorBloat;

public class OrderNotificationService
{
    private readonly CustomerNotifier _customerNotifier;
    private readonly WarehouseNotifier _warehouseNotifier;

    public OrderNotificationService(WarehouseNotifier warehouseNotifier, CustomerNotifier customerNotifier)
    {
        _warehouseNotifier = warehouseNotifier;
        _customerNotifier = customerNotifier;
    }

    public void NotifyOrderProcessed(Order order)
    {
        _customerNotifier.NotifyCustomer(order);
        _warehouseNotifier.NotifyWarehouse(order);
    }
}