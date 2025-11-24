using Cafe.Domain.Beverages;
using Cafe.Domain.Factories;

namespace Cafe.Infrastructure.Factories
{
    public class BeverageFactory : IBeverageFactory
    {
        public IBeverage Create(string key)
        {
            key = (key ?? "").Trim().ToLowerInvariant();

            switch (key)
            {
                case "1":
                case "espresso":
                    return new Espresso();

                case "2":
                case "tea":
                    return new Tea();

                case "3":
                case "choc":
                case "hotchocolate":
                    return new HotChocolate();

                default:
                    throw new ArgumentException($"Unknown beverage key '{key}'");
            }
        }
    }
}
