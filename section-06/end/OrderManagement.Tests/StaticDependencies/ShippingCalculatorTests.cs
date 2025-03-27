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
        
        var distanceCalculator = new FakeDistanceCalculator(50.0M);
        var centerLocator = new FakeCenterFinder("TEST-CENTER");

        var calculator = new ShippingCalculator(distanceCalculator, centerLocator);
        
        // Act
        var cost = calculator.CalculateShippingCost(order);
        
        // Assert
        Assert.Equal(12.94M, cost);
    }
    
    [Fact]
    public void CalculateShippingCost_WithZeroDistance_ShouldReturnMinimumCharge()
    {
        // Arrange
        var distanceCalculator = new FakeDistanceCalculator(0.0M); // Zero distance
        var centerLocator = new FakeCenterFinder("LOCAL-CENTER");
        
        var calculator = new ShippingCalculator(distanceCalculator, centerLocator);
        
        var order = new Order
        {
            Id = 123,
            ShippingAddress = new Address { ZipCode = "12345" },
            TotalWeight = 15, // Below threshold
            ContainsFragileItems = false
        };
        
        // Act
        var cost = calculator.CalculateShippingCost(order);
        
        // Assert
        Assert.Equal(5.00m, cost); // Just the base cost
    }
    
    private class FakeDistanceCalculator : IDistanceCalculator
    {
        private readonly decimal _fixedDistance;
        
        public FakeDistanceCalculator(decimal fixedDistance)
        {
            _fixedDistance = fixedDistance;
        }
        
        public decimal CalculateDistance(string originZip, string destinationZip)
        {
            return _fixedDistance;
        }
    }
    
    private class FakeCenterFinder : IDistributionCenterFinder
    {
        private readonly string _centerCode;
        
        public FakeCenterFinder(string centerCode)
        {
            _centerCode = centerCode;
        }
        
        public string GetNearestCenter(Address shippingAddress)
        {
            return _centerCode;
        }
    }


}