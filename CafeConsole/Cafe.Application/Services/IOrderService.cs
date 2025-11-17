using Cafe.Domain.Beverages;
using Cafe.Domain.Pricing;
using Cafe.Domain.Events;
using Cafe.Infrastructure.Observers;

namespace Cafe.Application.Services
{
    public interface IOrderService
    {
        IBeverage CreateBase(string key);
        IBeverage AddMilk(IBeverage beverage);
        IBeverage AddSyrup(IBeverage beverage, string flavor);
        IBeverage AddExtraShot(IBeverage beverage);
        (decimal subtotal, decimal total, string description) FinalizeOrder(IBeverage beverage, IPricingStrategy strategy);
        InMemoryOrderAnalytics Analytics { get; }
    }
}
