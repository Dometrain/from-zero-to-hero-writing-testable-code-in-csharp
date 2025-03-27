namespace Exercises;

public class PromotionManager : IPromotionManager
{
    private decimal _currentDiscountPercentage = 0;
    private bool _specialOfferActive = false;

    public decimal GetCurrentDiscountPercentage()
    {
        return _currentDiscountPercentage;
    }

    public bool IsSpecialOfferActive()
    {
        return _specialOfferActive;
    }

    public void StartPromotion(decimal discountPercentage)
    {
        _currentDiscountPercentage = discountPercentage;
        _specialOfferActive = true;
    }

    public void EndPromotion()
    {
        _currentDiscountPercentage = 0;
        _specialOfferActive = false;
    }
}