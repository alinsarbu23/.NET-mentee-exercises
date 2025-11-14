using Cafe.Domain.Beverages;
using Cafe.Infrastructure.Factories;

namespace Cafe.Tests
{
    public class FactoryTests
    {
        [Theory]
        [InlineData("espresso", typeof(Espresso))]
        [InlineData("tea", typeof(Tea))]
        [InlineData("choc", typeof(HotChocolate))]
        public void Factory_Returns_Type(string key, Type expected)
        {
            var factoryType = new BeverageFactory();
            var beverage = factoryType.Create(key);
            Assert.IsType(expected, beverage);
        }

        [Fact]
        public void Factory_InvalidKey_Throws_ArgumentException()
        {
            var factory = new BeverageFactory();
            Assert.Throws<ArgumentException>(() => factory.Create("invalid"));
        }
    }
}
