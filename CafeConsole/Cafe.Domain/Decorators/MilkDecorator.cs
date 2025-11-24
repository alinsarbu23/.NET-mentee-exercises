using Cafe.Domain.Beverages;

namespace Cafe.Domain.Decorators
{
    public class MilkDecorator : BeverageDecorator
    {
        public MilkDecorator(IBeverage baseBeverage) : base(baseBeverage) { }

        public override decimal Cost()
        {
            return BaseBeverage.Cost() + 0.40m; ;
        }

        public override string Describe()
        {
            return BaseBeverage.Describe() + " - milk";
        }

    }
}
