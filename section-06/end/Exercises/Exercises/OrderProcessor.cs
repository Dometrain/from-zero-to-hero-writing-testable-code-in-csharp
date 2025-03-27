namespace Exercises;

public class OrderProcessor
{
    private readonly OrderCalculator _calculator = new ();
    private readonly IInventorySystem _inventorySystem;
    private readonly IPromotionManager _promotionManager;
    private readonly IAuditLogger _auditLogger;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserContext _userContext;

    public OrderProcessor(
        IInventorySystem inventorySystem,
        IPromotionManager promotionManager,
        IAuditLogger auditLogger,
        IDateTimeProvider dateTimeProvider,
        IUserContext userContext)
    {
        _inventorySystem = inventorySystem;
        _promotionManager = promotionManager;
        _auditLogger = auditLogger;
        _dateTimeProvider = dateTimeProvider;
        _userContext = userContext;
    }

    public bool ProcessOrder(Order order)
    {
        var total = _calculator.CalculateOrderTotal(
            order,
            _promotionManager.IsSpecialOfferActive() ? _promotionManager.GetCurrentDiscountPercentage() : 0
        );
        
        order.TotalAmount = total;
        
        foreach (var item in order.Items)
        {
            item.LineTotal = item.Price * item.Quantity;

            if (!_inventorySystem.CheckStock(item.ProductId, item.Quantity))
            {
                order.Status = OrderStatus.Cancelled;
                return false;
            }
        }

        // Deduct from inventory
        foreach (var item in order.Items)
        {
            _inventorySystem.DeductStock(item.ProductId, item.Quantity);
        }
        
        order.Status = OrderStatus.Completed;
        order.ProcessedAt = _dateTimeProvider.GetCurrentTime();
        
        _auditLogger.LogOrderProcessed(order, _userContext.GetUsername());

        return true;
    }
}