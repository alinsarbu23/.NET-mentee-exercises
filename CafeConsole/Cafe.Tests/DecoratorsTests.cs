using Cafe.Domain.Beverages;
using Cafe.Domain.Decorators;
using Xunit;

namespace Cafe.Tests
{
    public class DecoratorsTests
    {
        [Fact]
        public void Decorator_AddsCost_AndDescription()
        {
            // Arrange
            IBeverage beverage = new Espresso();

            // Act
            beverage = new MilkDecorator(beverage);
            beverage = new ExtraShotDecorator(beverage);

            // Assert
            Assert.Equal(3.70m, beverage.Cost());
            var description = beverage.Describe().ToLowerInvariant();
            Assert.Contains("milk", description);
            Assert.Contains("extra shot", description);
        }
    }
}
