namespace Exercises;

public static class PromotionEngine
{
    public static decimal CurrentDiscountPercentage { get; set; } = 0;
    public static bool SpecialOfferActive { get; set; } = false;

    public static void StartPromotion(decimal discountPercentage)
    {
        CurrentDiscountPercentage = discountPercentage;
        SpecialOfferActive = true;
    }

    public static void EndPromotion()
    {
        CurrentDiscountPercentage = 0;
        SpecialOfferActive = false;
    }
}