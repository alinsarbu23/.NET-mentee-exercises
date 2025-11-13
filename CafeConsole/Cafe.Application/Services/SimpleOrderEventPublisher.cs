using Cafe.Domain.Events;

namespace Cafe.Application.Services
{
    public class SimpleOrderEventPublisher : IOrderEventPublisher
    {
        private readonly List<IOrderEventSubscriber> subscribers = new List<IOrderEventSubscriber>();

        public void Subscribe(IOrderEventSubscriber subscriber)
        {
            subscribers.Add(subscriber);
        }

        public void Publish(OrderPlaced evt)
        {
            foreach (var subscriber in subscribers)
            {
                subscriber.On(evt);
            }
        }
    }
}
