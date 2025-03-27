using OrderManagement.ImplementingPureFunctions;

namespace OrderManagement.Tests.ImplementingPureFunctions;

public class OrderTaxCalculatorTests
{
    private readonly OrderTaxCalculator _calculator = new();

    #region CalculateItemTax Tests

    [Fact]
    public void CalculateItemTax_WithFoodItem_ShouldApply5PercentTax()
    {
        // Arrange
        var item = new OrderItem
        {
            ProductId = 1,
            ProductType = ProductType.Food,
            Price = 10.00m,
            Quantity = 2
        };

        // Act
        var tax = _calculator.CalculateItemTax(item);

        // Assert
        Assert.Equal(1.00m, tax); // 10.00 * 2 * 0.05 = 1.00
    }

    [Fact]
    public void CalculateItemTax_WithClothingItem_ShouldApply8PercentTax()
    {
        // Arrange
        var item = new OrderItem
        {
            ProductId = 2,
            ProductType = ProductType.Clothing,
            Price = 50.00m,
            Quantity = 1
        };

        // Act
        var tax = _calculator.CalculateItemTax(item);

        // Assert
        Assert.Equal(4.00m, tax); // 50.00 * 1 * 0.08 = 4.00
    }

    [Fact]
    public void CalculateItemTax_WithElectronicsItem_ShouldApply10PercentTax()
    {
        // Arrange
        var item = new OrderItem
        {
            ProductId = 3,
            ProductType = ProductType.Electronics,
            Price = 100.00m,
            Quantity = 1
        };

        // Act
        var tax = _calculator.CalculateItemTax(item);

        // Assert
        Assert.Equal(10.00m, tax); // 100.00 * 1 * 0.10 = 10.00
    }

    [Fact]
    public void CalculateItemTax_WithOtherProductType_ShouldApplyDefaultTax()
    {
        // Arrange
        var item = new OrderItem
        {
            ProductId = 4,
            ProductType = ProductType.Other,
            Price = 20.00m,
            Quantity = 1
        };

        // Act
        var tax = _calculator.CalculateItemTax(item);

        // Assert
        Assert.Equal(1.40m, tax); // 20.00 * 1 * 0.07 = 1.40
    }

    [Fact]
    public void CalculateItemTax_WithZeroQuantity_ShouldReturnZero()
    {
        // Arrange
        var item = new OrderItem
        {
            ProductId = 5,
            ProductType = ProductType.Electronics,
            Price = 200.00m,
            Quantity = 0
        };

        // Act
        var tax = _calculator.CalculateItemTax(item);

        // Assert
        Assert.Equal(0m, tax);
    }

    #endregion

    #region CalculateTotalTax Tests

    [Fact]
    public void CalculateTotalTax_WithEmptyOrder_ShouldReturnZero()
    {
        // Arrange
        var order = new Order
        {
            Items = new List<OrderItem>()
        };

        // Act
        var totalTax = _calculator.CalculateTotalTax(order);

        // Assert
        Assert.Equal(0m, totalTax);
    }

    [Fact]
    public void CalculateTotalTax_WithSingleItem_ShouldReturnItemTax()
    {
        // Arrange
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = 1,
                    ProductType = ProductType.Food,
                    Price = 10.00m,
                    Quantity = 2
                }
            }
        };

        // Act
        var totalTax = _calculator.CalculateTotalTax(order);

        // Assert
        Assert.Equal(1.00m, totalTax); // 10.00 * 2 * 0.05 = 1.00
    }

    [Fact]
    public void CalculateTotalTax_WithMultipleItems_ShouldReturnSumOfItemTaxes()
    {
        // Arrange
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = 1,
                    ProductType = ProductType.Food,
                    Price = 10.00m,
                    Quantity = 2
                },
                new OrderItem
                {
                    ProductId = 2,
                    ProductType = ProductType.Electronics,
                    Price = 100.00m,
                    Quantity = 1
                },
                new OrderItem
                {
                    ProductId = 3,
                    ProductType = ProductType.Clothing,
                    Price = 25.00m,
                    Quantity = 2
                }
            }
        };

        // Act
        var totalTax = _calculator.CalculateTotalTax(order);

        // Assert
        // Food: 10.00 * 2 * 0.05 = 1.00
        // Electronics: 100.00 * 1 * 0.10 = 10.00
        // Clothing: 25.00 * 2 * 0.08 = 4.00
        // Total: 15.00
        Assert.Equal(15.00m, totalTax);
    }

    #endregion

    #region ApplyTaxes Tests

    [Fact]
    public void ApplyTaxes_ShouldUpdateTaxAmountsAndReturnTotal()
    {
        // Arrange
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = 1,
                    ProductType = ProductType.Food,
                    Price = 10.00m,
                    Quantity = 2,
                    TaxAmount = 0 // Initially zero
                },
                new OrderItem
                {
                    ProductId = 2,
                    ProductType = ProductType.Electronics,
                    Price = 100.00m,
                    Quantity = 1,
                    TaxAmount = 0 // Initially zero
                }
            },
            TaxAmount = 0 // Initially zero
        };

        // Act
        var returnedTax = _calculator.ApplyTaxes(order);

        // Assert
        // Check the returned value
        Assert.Equal(11.00m, returnedTax);

        // Check that order tax was updated
        Assert.Equal(11.00m, order.TaxAmount);

        // Check that item taxes were updated
        Assert.Equal(1.00m, order.Items[0].TaxAmount);
        Assert.Equal(10.00m, order.Items[1].TaxAmount);
    }

    [Fact]
    public void ApplyTaxes_WithEmptyOrder_ShouldSetZeroTax()
    {
        // Arrange
        var order = new Order
        {
            Items = new List<OrderItem>(),
            TaxAmount = 10.00m // Some non-zero value
        };

        // Act
        var returnedTax = _calculator.ApplyTaxes(order);

        // Assert
        Assert.Equal(0m, returnedTax);
        Assert.Equal(0m, order.TaxAmount);
    }

    [Fact]
    public void ApplyTaxes_CalledMultipleTimes_ShouldUpdateValuesConsistently()
    {
        // Arrange
        var order = new Order
        {
            Items = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = 1,
                    ProductType = ProductType.Clothing,
                    Price = 50.00m,
                    Quantity = 1,
                    TaxAmount = 0
                }
            },
            TaxAmount = 0
        };

        // Act - first call
        _calculator.ApplyTaxes(order);

        // Save intermediate state
        var firstItemTax = order.Items[0].TaxAmount;
        var firstOrderTax = order.TaxAmount;

        // Change price and apply taxes again
        order.Items[0].Price = 75.00m;

        // Act - second call
        _calculator.ApplyTaxes(order);

        // Assert
        // First call should have set correct values
        Assert.Equal(4.00m, firstItemTax); 
        Assert.Equal(4.00m, firstOrderTax);

        // Second call should have updated values based on new price
        Assert.Equal(6.00m, order.Items[0].TaxAmount);
        Assert.Equal(6.00m, order.TaxAmount);
    }

    #endregion
}