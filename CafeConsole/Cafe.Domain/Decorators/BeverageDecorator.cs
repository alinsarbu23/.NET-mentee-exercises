using Cafe.Domain.Beverages;

namespace Cafe.Domain.Decorators
{
    public abstract class BeverageDecorator : IBeverage
    {
        protected readonly IBeverage BaseBeverage;

        public BeverageDecorator(IBeverage beverage)
        {
            BaseBeverage = beverage;
        }

        public virtual string Name => BaseBeverage.Name;
        public virtual decimal Cost() => BaseBeverage.Cost();
        public virtual string Describe() => BaseBeverage.Name;
    }
}
