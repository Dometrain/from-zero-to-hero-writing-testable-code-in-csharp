using OrderManagement.FrameworkCode;

namespace OrderManagement.Tests.FrameworkCode;

public class OrderNumberGeneratorTests
{
    [Fact]
    public void GenerateOrderNumber_ReturnsCorrectFormat()
    {
        // Arrange
        var generator = new OrderNumberGenerator();
        
        // Act
        var orderNumber = generator.GenerateOrderNumber();
        
        // Assert
        Assert.Matches(@"^ORD-\d{14}-\d{4}$", orderNumber);
        
        // PROBLEM: This test only verifies the format, not the actual behavior
        // We can't verify that:
        // 1. The timestamp part uses the current time
        // 2. The random part is between 1000-9999
        // 3. The method produces unique values on each call
    }
    
    [Fact]
    public void GenerateOrderNumber_ReturnsUniqueValues()
    {
        // Arrange
        var generator = new OrderNumberGenerator();
        
        // Act
        var number1 = generator.GenerateOrderNumber();
        
        // Need to wait to ensure timestamp changes
        Thread.Sleep(1000);
        
        var number2 = generator.GenerateOrderNumber();
        
        // Assert
        Assert.NotEqual(number1, number2);
        
        // PROBLEM: This test:
        // 1. Is time-dependent and slows down the test suite
        // 2. Could still fail if both calls happen in the same second
        // 3. We can't control or verify the randomness behavior
        // 4. We're testing the framework's Random class and DateTime,
        //    not our business logic
    }
}