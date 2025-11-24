using Cafe.Application.Services;
using Cafe.ConsoleUI.Utils;
using Cafe.Domain.Beverages;
using Cafe.Domain.Pricing;

namespace Cafe.ConsoleUI.Menu
{
    public sealed class CafeMenu
    {
        private readonly IOrderService _service;
        private readonly string _currency = "$";

        public CafeMenu(IOrderService service) => _service = service;

        public void Run()
        {
            while (true)
            {
                var action = ShowMainMenu();

                if (action == MainAction.Exit)
                {
                    break;
                }

                if (action == MainAction.NewOrder)
                {
                    RunOrderSubmenu();
                    continue;
                }

                if (action == MainAction.Analytics)
                {
                    ShowAnalyticsSubmenu();
                    continue;
                }
            }
        }

        private MainAction ShowMainMenu()
        {

            Console.WriteLine("=== Cafe Console ===");
            Console.WriteLine("1) New order");
            Console.WriteLine("2) Analytics");
            Console.WriteLine("0) Exit");

            while (true)
            {
                Console.Write("Choose: ");
                var input = (Console.ReadLine() ?? "").Trim();

                if (input == "1")
                {
                    return MainAction.NewOrder;
                }
                if (input == "2")
                {
                    return MainAction.Analytics;
                }
                if (input == "0")
                {
                    return MainAction.Exit;
                }
                Console.WriteLine("Invalid option.");
            }
        }

        private void RunOrderSubmenu()
        {
            var baseDrink = ChooseBase();
            var finalDrink = ChooseAddOns(baseDrink);
            var strategy = ChoosePricingStrategy();

            var result = _service.FinalizeOrder(finalDrink, strategy);

            PrintReceipt(result.description, result.subtotal, result.total, strategy.Name);
            Console.WriteLine($"\nAnalytics: orders={_service.Analytics.OrdersCount}, revenue={_currency}{Math.Round(_service.Analytics.Revenue, 2):F2}\n");

            if (!ConsoleHelper.PromptYesNo("Place another order? (y/n): "))
            {
                Console.Clear();
            }
        }

        private void ShowAnalyticsSubmenu()
        {
            Console.WriteLine("\n=== Analytics ===");
            Console.WriteLine($"Orders:  {_service.Analytics.OrdersCount}");
            Console.WriteLine($"Revenue: {_currency}{Math.Round(_service.Analytics.Revenue, 2):F2}\n");
            Console.WriteLine("Press ENTER to return to main menu...");
            Console.ReadLine();
            Console.Clear();
        }

        private IBeverage ChooseBase()
        {
            Console.WriteLine(@"Choose base beverage:
                1) Espresso ($2.50)
                2) Tea ($2.00)
                3) Hot Chocolate ($3.00)");

            while (true)
            {
                Console.Write("Your choice [1-3]: ");
                var input = Console.ReadLine()?.Trim() ?? "";

                try 
                { 
                    return _service.CreateBase(input); 
                }
                catch { Console.WriteLine("Invalid choice. Try 1, 2 or 3."); }
            }
        }

        private IBeverage ChooseAddOns(IBeverage b)
        {
            while (true)
            {
                Console.WriteLine("\nAdd-ons (0 = Done):");
                Console.WriteLine("  1) Milk (+0.40)");
                Console.WriteLine("  2) Syrup (+0.50)");
                Console.WriteLine("  3) Extra shot (+0.80)");
                Console.Write("Your choice: ");
                var input = (Console.ReadLine() ?? "").Trim();

                if (input == "0")
                {
                    return b;
                }
                if (input == "1")
                { 
                    b = _service.AddMilk(b);
                    continue; 
                }
                if (input == "2")
                {
                    Console.Write("Flavor (e.g., vanilla): ");
                    var flavor = (Console.ReadLine() ?? "").Trim();
                    b = _service.AddSyrup(b, flavor);
                    continue;
                }
                if (input == "3") 
                { 
                    b = _service.AddExtraShot(b);
                    continue; 
                }

                Console.WriteLine("Invalid option.");
            }
        }

        private IPricingStrategy ChoosePricingStrategy()
        {
            Console.WriteLine("\nPricing policy:");
            Console.WriteLine("  1) Regular");
            Console.WriteLine("  2) Happy Hour (-20%)");
            while (true)
            {
                Console.Write("Your choice [1-2]: ");
                var input = Console.ReadLine()?.Trim();

                if (input == "1")
                {
                    return new RegularPricing();
                }

                if (input == "2")
                {
                    return new HappyHourPricing();
                }
                Console.WriteLine("Invalid choice.");
            }
        }

        private void PrintReceipt(string desc, decimal subtotal, decimal total, string pricing)
        {
            Console.WriteLine("\n===== Receipt =====");
            Console.WriteLine($"Items: {desc}");
            Console.WriteLine($"Subtotal: {_currency}{subtotal:F2}");

            if (pricing.Equals("HappyHour", StringComparison.OrdinalIgnoreCase))
            {
                var discount = subtotal - total;
                Console.WriteLine($"Pricing: {pricing} (-{discount:F2})");
            }

            else
            {
                Console.WriteLine($"Pricing: {pricing}");
            }
            Console.WriteLine($"Total: {_currency}{Math.Round(total, 2):F2}");
            Console.WriteLine("===================\n");
        }

        private enum MainAction
        {
            NewOrder,
            Analytics,
            Exit
        }
    }
}
