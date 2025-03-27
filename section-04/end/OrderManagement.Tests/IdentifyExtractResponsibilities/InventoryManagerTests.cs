using OrderManagement.IdentifyExtractResponsibilities;

namespace OrderManagement.Tests.IdentifyExtractResponsibilities;

public class InventoryManagerTests
{
    private readonly InventoryManager _inventoryManager = new();

    [Fact]
    public void UpdateInventory_ShouldDecrementStockQuantity()
    {
        // Arrange
        var product = new Product { StockQuantity = 20 };
        var items = new List<OrderItem>
        {
            new() { Product = product, Quantity = 5 }
        };

        // Act
        _inventoryManager.UpdateInventory(items);

        // Assert
        Assert.Equal(15, product.StockQuantity);
    }

    [Fact]
    public void UpdateInventory_WhenStockBelow10_ShouldSetLowStockStatus()
    {
        // Arrange
        var product = new Product { StockQuantity = 12 };
        var items = new List<OrderItem>
        {
            new() { Product = product, Quantity = 5 }
        };

        // Act
        _inventoryManager.UpdateInventory(items);

        // Assert
        Assert.Equal(StockStatus.LowStock, product.StockStatus);
    }

    [Fact]
    public void UpdateInventory_WithMultipleItems_ShouldUpdateAllProducts()
    {
        // Arrange
        var product1 = new Product { StockQuantity = 20 };
        var product2 = new Product { StockQuantity = 15 };
        var items = new List<OrderItem>
        {
            new() { Product = product1, Quantity = 5 },
            new() { Product = product2, Quantity = 8 }
        };

        // Act
        _inventoryManager.UpdateInventory(items);

        // Assert
        Assert.Equal(15, product1.StockQuantity);
        Assert.Equal(7, product2.StockQuantity);
        Assert.Equal(StockStatus.LowStock, product2.StockStatus);
    }
}