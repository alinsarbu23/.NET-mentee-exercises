using Cafe.Domain.Beverages;

namespace Cafe.Domain.Decorators
{
    public class SyrupDecorator : BeverageDecorator
    {
        public string Flavour { get; }

        public SyrupDecorator(IBeverage BaseBeverage, string flavour) : base(BaseBeverage)
        {
            Flavour = flavour;
        }

        public override decimal Cost()
        {
            return BaseBeverage.Cost() + 0.50m;
        }

        public override string Describe()
        {
            return BaseBeverage.Describe() + $" with flavor {Flavour.ToLowerInvariant()}";
        }
    }
}
