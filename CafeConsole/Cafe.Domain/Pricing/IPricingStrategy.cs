namespace Cafe.Domain.Pricing
{
    public interface IPricingStrategy
    {
        decimal Apply(decimal subtotal);
        string Name { get; }
    }
}
