using Cafe.Domain.Beverages;
using Cafe.Domain.Decorators;
using Cafe.Domain.Events;
using Cafe.Domain.Factories;
using Cafe.Domain.Pricing;
using Cafe.Infrastructure.Factories;
using Cafe.Infrastructure.Observers;

namespace Cafe.Application.Services
{
    public sealed class OrderService : IOrderService
    {
        private readonly IBeverageFactory _factory;
        private readonly IOrderEventPublisher _publisher;
        private readonly InMemoryOrderAnalytics _analytics;
        private readonly IOrderEventSubscriber _logger;

        public InMemoryOrderAnalytics Analytics => _analytics;

        public OrderService()
        {
            _factory = new BeverageFactory();
            _publisher = new SimpleOrderEventPublisher();
            _analytics = new InMemoryOrderAnalytics();
            _logger = new OrderConsoleLogger();

            _publisher.Subscribe(_logger);
            _publisher.Subscribe(_analytics);
        }

        public IBeverage CreateBase(string key) => _factory.Create(key);
        public IBeverage AddMilk(IBeverage b) => new MilkDecorator(b);
        public IBeverage AddSyrup(IBeverage b, string flavor) => new SyrupDecorator(b, flavor);
        public IBeverage AddExtraShot(IBeverage b) => new ExtraShotDecorator(b);

        public (decimal subtotal, decimal total, string description) FinalizeOrder(IBeverage beverage, IPricingStrategy strategy)
        {
            var subtotal = beverage.Cost();
            var total = strategy.Apply(subtotal);
            var desc = beverage.Describe();

            var order = new OrderPlaced(Guid.NewGuid(), DateTimeOffset.Now, desc, subtotal, total);
            _publisher.Publish(order);

            return (subtotal, total, desc);
        }
    }
}
