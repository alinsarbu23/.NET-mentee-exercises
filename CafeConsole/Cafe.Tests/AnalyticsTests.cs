using System;
using Cafe.Domain.Events;
using Cafe.Infrastructure.Observers;
using Xunit;

namespace Cafe.Tests
{
    public class AnalyticsTests
    {
        [Fact]
        public void Orders_Are_Published_Analytics_Tracks_Count_And_Revenue()
        {
            // Arrange
            var analytics = new InMemoryOrderAnalytics();
            var order1 = new OrderPlaced(Guid.NewGuid(), DateTimeOffset.Now, "Espresso", 3.50m, 3.50m);
            var order2 = new OrderPlaced(Guid.NewGuid(), DateTimeOffset.Now, "Tea", 2.00m, 2.00m);

            // Act
            analytics.On(order1);
            analytics.On(order2);

            // Assert
            Assert.Equal(2, analytics.OrdersCount);
            Assert.Equal(5.50m, analytics.Revenue);
        }
    }
}
