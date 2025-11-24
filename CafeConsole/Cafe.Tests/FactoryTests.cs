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
        public void Factory_Returns_Correct_Type(string key, Type expected)
        {
            // Arrange
            var factory = new BeverageFactory();

            // Act
            var beverage = factory.Create(key);

            // Assert
            Assert.IsType(expected, beverage);
        }

        [Fact]
        public void Factory_InvalidKey_Throws_ArgumentException()
        {
            // Arrange
            var factory = new BeverageFactory();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => factory.Create("invalid"));
        }
    }
}
