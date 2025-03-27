using OrderManagement.CompositionOfNarrowComponents;

namespace OrderManagement.Tests.CompositionOfNarrowComponents;

public class InventoryValidatorTests
{
    private readonly FakeProductRepository _productRepository;
    private readonly InventoryValidator _validator;

    public InventoryValidatorTests()
    {
        _productRepository = new FakeProductRepository();
        _validator = new InventoryValidator(_productRepository);
    }

    [Fact]
    public void HasSufficientInventory_WhenEnoughStock_ReturnsTrue()
    {
        // Arrange
        const int productId = 1;
        const int requestedQuantity = 5;
        _productRepository.SetInventoryLevel(productId, 10);

        // Act
        var result = _validator.HasSufficientInventory(productId, requestedQuantity);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasSufficientInventory_WhenNotEnoughStock_ReturnsFalse()
    {
        // Arrange
        const int productId = 1;
        const int requestedQuantity = 10;
        _productRepository.SetInventoryLevel(productId, 5);

        // Act
        var result = _validator.HasSufficientInventory(productId, requestedQuantity);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasSufficientInventory_WhenExactStock_ReturnsTrue()
    {
        // Arrange
        const int productId = 1;
        const int requestedQuantity = 5;
        _productRepository.SetInventoryLevel(productId, 5);

        // Act
        var result = _validator.HasSufficientInventory(productId, requestedQuantity);

        // Assert
        Assert.True(result);
    }
} 