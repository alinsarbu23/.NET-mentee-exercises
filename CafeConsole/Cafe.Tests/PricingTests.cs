using Cafe.Domain.Pricing;
using Xunit;

namespace Cafe.Tests
{
    public class PricingTests
    {
        [Fact]
        public void Regular_WhenApply_10_Returns_10()
        {
            // Arrange
            var pricing = new RegularPricing();

            // Act
            var total = pricing.Apply(10.00m);

            // Assert
            Assert.Equal(10.00m, total);
        }

        [Fact]
        public void HappyHour_WhenApply_10_Returns_8()
        {
            // Arrange
            var pricing = new HappyHourPricing();

            // Act
            var total = pricing.Apply(10.00m);

            // Assert
            Assert.Equal(8.00m, total);
        }
    }
}
