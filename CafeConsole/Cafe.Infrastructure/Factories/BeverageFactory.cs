using Cafe.Domain.Beverages;
using Cafe.Domain.Factories;

namespace Cafe.Infrastructure.Factories
{
    public class BeverageFactory : IBeverageFactory
    {
        public IBeverage Create(string key)
        {
            if (key == null) return new Espresso();

            key = key.Trim().ToLowerInvariant();

            if (key == "1" || key == "espresso")
                return new Espresso();

            if (key == "2" || key == "tea")
                return new Tea();

            if (key == "3" || key == "choc" || key == "hotchocolate")
                return new HotChocolate();

            throw new ArgumentException($"Unknown beverage key '{key}'");
        }
    }
}



