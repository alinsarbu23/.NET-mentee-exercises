using System.Globalization;
using MiniBank.Models;
using MiniBank.Models.Interfaces;
using MiniBank.Services;

namespace MiniBank
{
    public class Program
    {
        static AccountRegistry registry = new();

        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;

            while (true)
            {
                Console.WriteLine("\n=== MINI BANK ===");
                Console.WriteLine("1. List accounts");
                Console.WriteLine("2. Create account");
                Console.WriteLine("3. Deposit");
                Console.WriteLine("4. Withdraw");
                Console.WriteLine("5. Transfer");
                Console.WriteLine("6. Month-end");
                Console.WriteLine("7. Exit");
                Console.Write("Choice: ");
                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    ListAccounts();
                }
                else if (choice == "2")
                {
                    CreateAccount();
                }
                else if (choice == "3")
                {
                    DoTransaction(true);
                }
                else if (choice == "4")
                {
                    DoTransaction(false);
                }
                else if (choice == "5")
                {
                    DoTransfer();
                }
                else if (choice == "6")
                {
                    RunMonthEnd();
                }
                else if (choice == "7")
                {
                    break;
                }
                else Console.WriteLine("Invalid option!");
            }
        }


        static void ListAccounts()
        {
            if (!registry.Accounts.Any()) 
            {
                Console.WriteLine("No accounts created.");
                return; 
            }
            foreach (var a in registry.Accounts)
            {
                Console.WriteLine($"#{a.Id} | {a.Owner} | {a.GetType().Name} | {a.Balance:C}");
            }

        }

        static void CreateAccount()
        {
            Console.Write("Type (Checking/Savings/Loan): ");
            string? type = Console.ReadLine()?.Trim().ToLower();
            Console.Write("Owner: ");
            string? owner = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(owner)) 
            {   Console.WriteLine("Invalid owner!"); 
                return; 
            }

            Console.Write("Initial amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            { 
                Console.WriteLine("Invalid amount."); 
                return; 
            }

            int id = registry.NextId();
            BankAccount acc;

            if (type == "checking")
            {
                acc = new CheckingAccount(id, owner, amount);
            }
            else if (type == "savings")
            {
                acc = new SavingsAccount(id, owner, amount);
            }
            else if (type == "loan")
            {
                acc = new LoanAccount(id, owner, amount);
            }
            else 
            { 
                Console.WriteLine("Invalid type!");
                return; 
            }

            registry.Add(acc);
            Console.WriteLine($"Account #{id} created for {owner} ({acc.GetType().Name}) with {amount:C}");
        }

        static void DoTransaction(bool isDeposit)
        {
            Console.Write("Account ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var acc = registry.FindById(id);
            if (acc is null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.Write("Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            bool ok;
            string? err;

            if (isDeposit)
                ok = acc.Deposit(amount, out err);
            else
                ok = acc.Withdraw(amount, out err);

            Console.WriteLine(ok ? $"OK. New balance: {acc.Balance:C}" : err);
        }


        static void DoTransfer()
        {
            Console.Write("From ID: ");
            if (!int.TryParse(Console.ReadLine(), out int from)) 
            { 
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.Write("To ID: ");
            if (!int.TryParse(Console.ReadLine(), out int to)) 
            { 
                Console.WriteLine("Invalid ID.");
                return; 
            }

            Console.Write("Amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            { 
                Console.WriteLine("Invalid amount.");
                return;
            }

            if (registry.Transfer(from, to, amount, out var err))
            {
                Console.WriteLine($"Transfer {amount:C} from #{from} to #{to} OK.");
            }

            else
            {
                Console.WriteLine($"Failed: {err}");
            }

        }

        static void RunMonthEnd()
        {
            foreach (var a in registry.Accounts)
            {
                if (a is IInterestBearing ib)
                {
                    ib.ApplyMonthlyInterest();
                }
                else
                {
                    a.ApplyMonthEnd();
                }
            }
            Console.WriteLine("Month-end processed.");
        }
    }
}
