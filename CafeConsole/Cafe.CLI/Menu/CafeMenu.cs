using Cafe.Application.Services;
using Cafe.Domain.Beverages;
using Cafe.Domain.Decorators;
using Cafe.Domain.Events;
using Cafe.Domain.Factories;
using Cafe.Domain.Pricing;
using Cafe.Infrastructure.Factories;
using Cafe.Infrastructure.Observers;

namespace Cafe.ConsoleUI.Menu
{
    public class CafeMenu
    {
        private readonly IBeverageFactory _factory = new BeverageFactory();
        private readonly SimpleOrderEventPublisher _publisher = new();
        private readonly InMemoryOrderAnalytics _analytics = new();
        private readonly OrderConsoleLogger _logger = new();
        private readonly string _currency = "$";

        public CafeMenu()
        {
            _publisher.Subscribe(_logger);
            _publisher.Subscribe(_analytics);
        }

        public void Run()
        {
            while(true)
            {
                var baseDrink = ChooseDrink();
                var finalDrink = ChooseFlavour(baseDrink);

                var strategy = ChoosePricingStrategy();

                var subtotal = finalDrink.Cost();
                var total = strategy.Apply(subtotal);
                var orderId = Guid.NewGuid();
                var at = DateTimeOffset.Now;
                var description = finalDrink.Describe();

                PrintReceipt(orderId, at, description, subtotal, total, strategy.Name);
                _publisher.Publish(new OrderPlaced(orderId, at, description, subtotal, total));

                Console.WriteLine();
                Console.WriteLine($"Analytics: OrdersCount: {_analytics.OrdersCount}, revenue: {_currency}{Math.Round(_analytics.Revenue, 2):F2}\n");

                if (!PromptYesNo("Place another order? (y/n): "))
                {
                    break;
                }
                Console.Clear();
            }
        }

        public IBeverage ChooseDrink()
        {
            Console.WriteLine("Choose base beverage:");
            Console.WriteLine("\t1) Espresso ($2.50)");
            Console.WriteLine("\t2) Tea ($2.00)");
            Console.WriteLine("\t3) Hot Chocolate ($3.00)");

            while (true)
            {
                Console.Write("Your choice (1-3): ");
                var input = Console.ReadLine()?.Trim() ?? "";

                try 
                { 
                    return _factory.Create(input); 
                }
                catch { Console.WriteLine("Invalid choice. Try 1, 2 or 3."); }
            }
        }

        private IBeverage ChooseFlavour(IBeverage beverage)
        {
            while(true)
            {
                Console.WriteLine();
                Console.WriteLine("Choose one flavour from bellow:");
                Console.WriteLine("\t1) Milk (+0.40)");
                Console.WriteLine("\t2) Syrup (+0.50)");
                Console.WriteLine("\t3) Extra shot (+0.80)");
                Console.WriteLine("\t0) Done");
                Console.WriteLine("Your option:");

                var input = (Console.ReadLine() ?? "").Trim();

                if (input == "0")
                {
                    return beverage;
                }
                if (input == "1")
                {
                    beverage = new MilkDecorator(beverage);
                    continue;
                }
                if (input == "2")
                {
                    Console.Write("Flavor (e.g. vanilla): ");
                    var flavor = (Console.ReadLine() ?? "").Trim();
                    beverage = new SyrupDecorator(beverage, flavor);
                    continue;
                }
                if (input == "3") 
                { 
                    beverage = new ExtraShotDecorator(beverage); 
                    continue; 
                }

                Console.WriteLine("Invalid option.");
            }
        }

        private IPricingStrategy ChoosePricingStrategy()
        {
            Console.WriteLine();
            Console.WriteLine("Choose pricing strategy (1-2)");
            Console.WriteLine("\t 1) Regular");
            Console.WriteLine("\t 2) HappyHour (-20%)");

            while(true)
            {
                Console.WriteLine("Your choice (1-2):");
                var input = Console.ReadLine()?.Trim();

                if (input == "1")
                {
                    return new RegularPricing();
                }
                if(input == "2")
                {
                    return new HappyHourPricing();
                }
                Console.WriteLine("Invalid input");
            }
        }

        private void PrintReceipt(Guid orderId, DateTimeOffset at, string description, decimal subtotal, decimal total, string pricingName)
        {
            Console.WriteLine();
            Console.WriteLine("===== Receipt =====");
            Console.WriteLine($"Order {orderId} - {at:o}");
            Console.WriteLine($"Description: {description}");
            Console.WriteLine($"Subtotal: {_currency}{subtotal:F2}");

            if (pricingName.Equals("HappyHour", StringComparison.OrdinalIgnoreCase))
            {
                var discount = subtotal - total;
                Console.WriteLine($"Pricing: {pricingName} (-{discount:F2})");
            }
            else
            {
                Console.WriteLine($"Pricing: {pricingName}");
            }
            Console.WriteLine($"Total: {_currency}{Math.Round(total, 2):F2}");
            Console.WriteLine("");
        }

        private static bool PromptYesNo(string message)
        {
            while (true)
            {
                Console.Write(message);
                var key = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

                if (key is "y" or "yes")
                {
                    return true;
                }
                if (key is "n" or "no")
                {
                    return false;
                }
                Console.WriteLine("Please answer y/n.");
            }
        }
    }
}
