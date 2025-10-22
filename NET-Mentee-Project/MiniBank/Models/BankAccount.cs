using MiniBank.Models.Interfaces;
using MiniBank.Services;

namespace MiniBank.Models
{
    public abstract class BankAccount : IStatement, ITransactable
    {
        public int Id { get; }
        public string Owner { get; }
        public decimal Balance { get; protected set; }

        private readonly AccountLog _records = new();

        protected BankAccount(int id, string owner, decimal initialBalance)
        {
            Id = id;
            Owner = owner.Trim();
            Balance = initialBalance;
            AddingMessage($"Account created for {Owner} with initial balance {Balance:C}");
        }

        protected void AddingMessage(string message)
        {
            _records.AddMessage(message);
        }

        public virtual bool Deposit(decimal amount, out string? error)
        {
            if (amount <= 0)
            {
                error = "Deposit amount must be positive.";
                return false;
            }

            Balance += amount;
            AddingMessage($"Deposited {amount:F2}, new balance is {Balance:F2}");
            error = null;
            return true;
        }

        public virtual bool Withdraw(decimal amount, out string? error)
        {
            if (amount <= 0)
            {
                error = "Withdrawal amount must be positive.";
                return false;
            }

            if (!CanWithdraw(amount, out error))
                return false;

            Balance -= amount;
            AddingMessage($"Withdrew {amount:F2}, new balance is {Balance:F2}");
            error = null;
            return true;
        }

        protected abstract bool CanWithdraw(decimal amount, out string? error);
        public abstract void ApplyMonthEnd();

        public void PrintStatement()
        {
            Console.WriteLine($"Account Statement for {Owner} (ID: {Id})");
            Console.WriteLine("--------------------------------------------------");
            _records.PrintRecords();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Current Balance: {Balance:C}");
        }

        protected bool NegativeBalanceCheck(decimal amount, decimal limit)
        {
            return (Balance - amount) < limit;
        }

        public void RestoreState(decimal balance, IEnumerable<string> records)
        {
            Balance = balance;
            _records.RestoreRecords(records);
        }

        public List<string> GetRecords()
        {
            return new List<string>(_records.GetRecords());
        }
    }
}
