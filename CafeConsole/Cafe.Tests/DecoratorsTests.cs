using Cafe.Domain.Beverages;
using Cafe.Domain.Decorators;

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
            IBeverage withMilk = new MilkDecorator(beverage);
            IBeverage withExtras = new ExtraShotDecorator(withMilk);

            // Assert
            Assert.Equal(3.70m, withExtras.Cost());
            var description = withExtras.Describe().ToLowerInvariant();
            Assert.Contains("milk", description);
            Assert.Contains("extra shot", description);
        }
    }
}
