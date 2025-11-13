using Cafe.Domain.Beverages;

namespace Cafe.Domain.Decorators
{
    public class ExtraShotDecorator : BeverageDecorator
    {
        public ExtraShotDecorator(IBeverage BaseBeverage) : base(BaseBeverage) { }

        public override decimal Cost()
        {
            return BaseBeverage.Cost() + 0.80m;
        }

        public override string Describe()
        {
            return BaseBeverage.Describe() + " - extra shot";
        }
    }
}
