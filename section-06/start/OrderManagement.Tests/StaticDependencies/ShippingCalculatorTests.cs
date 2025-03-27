using OrderManagement.StaticDependencies;

namespace OrderManagement.Tests.StaticDependencies;

public class ShippingCalculatorTests
{
    [Fact]
    public void CalculateShippingCost_WithStaticDependencies_IsHardToTest()
    {
        // Arrange
        var order = new Order
        {
            Id = 123,
            ShippingAddress = new Address { ZipCode = "12345" },
            TotalWeight = 25, // 5 pounds over the 20 pound threshold
            ContainsFragileItems = true
        };
        
        var calculator = new ShippingCalculator();
        
        // Act
        var cost = calculator.CalculateShippingCost(order);
        
        // Assert
        // This test is problematic for several reasons:
        
        // 1. We can't control what GeoDistanceService returns
        // 2. We can't verify that GeoDistanceService was called with the right parameters
        // 3. We can't control what DistributionCenterLocator returns
        // 4. If either static service changes behavior, our test breaks
        
        // We're forced to calculate the expected result based on what we know the static
        // dependencies will return, which makes our test brittle and coupled to implementation
        var expectedDistance = 42.5; // This is what we know GeoDistanceService will return
        var baseCost = 5.00m;
        var weightSurcharge = 5 * 0.25m; // 5 pounds over at $0.25 per pound
        var distanceCost = (decimal)expectedDistance * 0.1m;
        var fragileSurcharge = (baseCost + weightSurcharge + distanceCost) * 0.15m;
        var expectedTotal = baseCost + weightSurcharge + distanceCost + fragileSurcharge;
        expectedTotal = Math.Round(expectedTotal, 2);
        
        Assert.Equal(expectedTotal, cost);
        
        // If the implementation of ShippingCalculator changes even slightly,
        // or if GeoDistanceService or DistributionCenterLocator changes,
        // this test will break even if the business logic is still correct
    }
    
    // We can't even write a test for edge cases or error conditions
    // because we can't control what the static services return
    [Fact]
    public void CalculateShippingCost_WithZeroDistance_ShouldReturnMinimumCharge()
    {
        // We can't make GeoDistanceService return 0 for testing this edge case
        // This test is impossible to write without changing production code
        
        // This would require a redesign of ShippingCalculator to accept dependencies
        // that can be substituted in tests
    }
}