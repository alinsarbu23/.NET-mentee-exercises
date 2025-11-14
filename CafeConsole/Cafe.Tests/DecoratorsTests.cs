using Cafe.Domain.Beverages;
using Cafe.Domain.Decorators;

namespace Cafe.Tests
{
    public class DecoratorsTests
    {

        [Fact]
        public void Decorator_AddsCost_AndDescription()
        {
            IBeverage beverage = new Espresso();
            beverage = new MilkDecorator(beverage);
            beverage = new ExtraShotDecorator(beverage);

            Assert.Equal(3.70m, beverage.Cost());

            var description = beverage.Describe().ToLowerInvariant();
            Assert.Contains("milk", description);
            Assert.Contains("extra shot", description);
        }
    }
}
