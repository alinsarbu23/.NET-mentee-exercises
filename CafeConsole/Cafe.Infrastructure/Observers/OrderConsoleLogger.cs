using Cafe.Domain.Events;

namespace Cafe.Infrastructure.Observers
{
    public class OrderConsoleLogger : IOrderEventSubscriber
    {
        public void On(OrderPlaced evt)
        {
            Console.WriteLine($"Log: {evt.At} {evt.OrderId} {evt.Description} Subtotal: {evt.Subtotal:F2} Total: {evt.Total:F2}");
        }
    }
}
