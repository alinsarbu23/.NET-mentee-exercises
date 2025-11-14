using Cafe.Domain.Events;

namespace Cafe.Infrastructure.Observers
{
    public class InMemoryOrderAnalytics : IOrderEventSubscriber
    {
        public int OrdersCount { get; set; }
        public decimal Revenue { get; set; }

        public void On(OrderPlaced evt)
        {
            OrdersCount++;
            Revenue += evt.Total;
        }
    }
}
