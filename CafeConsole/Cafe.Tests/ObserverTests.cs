using Cafe.Application.Services;
using Cafe.Domain.Events;
using Moq;

namespace Cafe.Tests
{
    public class ObserverTests
    {
        [Fact]
        public void When_Event_Is_Published_Subscriber_Is_Notified()
        {
            // Arrange
            var publisher = new SimpleOrderEventPublisher();
            var mockSubscriber = new Mock<IOrderEventSubscriber>();
            publisher.Subscribe(mockSubscriber.Object);
            var order = new OrderPlaced(Guid.NewGuid(), DateTimeOffset.Now, "Espresso", 10m, 10m);

            // Act
            publisher.Publish(order);

            // Assert
            mockSubscriber.Verify(s => s.On(order), Times.Once);
        }
    }
}
