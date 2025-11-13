namespace Cafe.Domain.Pricing
{
    public class HappyHourPricing : IPricingStrategy
    {
        public decimal Apply(decimal subtotal)
        {
            return subtotal * 0.80m;
        }

        public string Name => "HappyHour";
    }
}
