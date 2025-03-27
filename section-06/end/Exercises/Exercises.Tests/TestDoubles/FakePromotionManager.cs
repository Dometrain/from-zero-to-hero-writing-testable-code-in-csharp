namespace Exercises.Tests.TestDoubles;

#region Test Doubles
    
// Test implementation of IPromotionManager
public class FakePromotionManager : IPromotionManager
{
    private decimal _discountPercentage;
    private bool _specialOfferActive;
        
    public FakePromotionManager(decimal discountPercentage = 0, bool specialOfferActive = false)
    {
        _discountPercentage = discountPercentage;
        _specialOfferActive = specialOfferActive;
    }
        
    public decimal GetCurrentDiscountPercentage() => _discountPercentage;
    public bool IsSpecialOfferActive() => _specialOfferActive;
    public void StartPromotion(decimal discountPercentage)
    {
        _discountPercentage = discountPercentage;
        _specialOfferActive = true;
    }
    public void EndPromotion()
    {
        _discountPercentage = 0;
        _specialOfferActive = false;
    }
}
    
// Test implementation of IInventorySystem

// Test implementation of IAuditLogger

// Test implementation of IDateTimeProvider

// Test implementation of IUserContext

#endregion
    
#region Tests

#endregion