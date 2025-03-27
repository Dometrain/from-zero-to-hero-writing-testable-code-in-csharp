namespace Exercises;

public interface IPromotionManager
{
    decimal GetCurrentDiscountPercentage();
    bool IsSpecialOfferActive();
    void StartPromotion(decimal discountPercentage);
    void EndPromotion();
}