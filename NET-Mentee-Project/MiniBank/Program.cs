using System.Globalization;
using MiniBank.Models;
using MiniBank.Models.Interfaces;
using MiniBank.Services;

namespace MiniBank
{
    public class Program
    {
        public static void Main()
        {
            var registry = new AccountRegistry();
            var app = new MiniBankConsoleApp(registry, "testBankAccounts.json");
            app.Run();
        }
    }
}
