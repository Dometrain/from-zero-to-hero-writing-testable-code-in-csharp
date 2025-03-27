namespace Exercises.Tests;

public class PromotionManagerTests
{
    private readonly PromotionManager _promotionManager;

    public PromotionManagerTests()
    {
        _promotionManager = new PromotionManager();
    }

    [Fact]
    public void StartPromotion_ShouldSetDiscountAndActivateOffer()
    {
        // Act
        _promotionManager.StartPromotion(15);
            
        // Assert
        Assert.Equal(15, _promotionManager.GetCurrentDiscountPercentage());
        Assert.True(_promotionManager.IsSpecialOfferActive());
    }
        
    [Fact]
    public void EndPromotion_ShouldResetDiscountAndDeactivateOffer()
    {
        // Arrange
        _promotionManager.StartPromotion(15);
            
        // Act
        _promotionManager.EndPromotion();
            
        // Assert
        Assert.Equal(0, _promotionManager.GetCurrentDiscountPercentage());
        Assert.False(_promotionManager.IsSpecialOfferActive());
    }
        
    [Fact]
    public void MultipleInstances_ShouldHaveIndependentState()
    {
        // Arrange
        var promotionManager1 = new PromotionManager();
        var promotionManager2 = new PromotionManager();
            
        // Act
        promotionManager1.StartPromotion(15);
            
        // Assert - manager2 should be unaffected by manager1
        Assert.Equal(15, promotionManager1.GetCurrentDiscountPercentage());
        Assert.True(promotionManager1.IsSpecialOfferActive());
        Assert.Equal(0, promotionManager2.GetCurrentDiscountPercentage());
        Assert.False(promotionManager2.IsSpecialOfferActive());
    }
}