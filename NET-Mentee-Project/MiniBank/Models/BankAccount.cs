using MiniBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Models
{

    /// <summary>
    /// Clasa de baza pentru toate conturile.
    /// - Stare comuna: Id, Owner, Balance
    /// - Jurnal al operatiunilor (privat), afisat prin PrintStatement()
    /// - Comportamente generale: Deposit, Withdraw (cu hook CanWithdraw), ApplyMonthEnd 
    /// - Clasele derivate definesc regulile de retragere in CanWithdraw(...)
    /// </summary>
    public abstract class BankAccount :ITransactable, IStatement
    {
        public int Id { get; }
        public string Owner { get; }
        public decimal Balance { get; protected set; }

        private readonly List<string> _records = new(); //for history

        protected BankAccount(int id, string owner, decimal initialBalance)
        {
            if(string.IsNullOrWhiteSpace(owner))
                throw new ArgumentException("Owner name cannot be empty.", nameof(owner));

            Id = id;
            Owner = owner.Trim();
            Balance = initialBalance;
            AddingMessage($"Account created for {Owner} with initial balance {Balance:C}");
        }

        protected void AddingMessage(string message)
        {
            DateTime dateTime = DateTime.Now;
            _records.Add($"[{dateTime:yyyy-MM-dd HH:mm:ss}] --> {message}");
        }

        public virtual bool Deposit(decimal amount, out string? error)
        {
            if(amount <= 0)
            {
                error = "Deposit amount must be positive.";
                return false;
            }

            Balance += amount;
            AddingMessage($"Deposited {amount:C}, new balance is {Balance:C}");
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
        public abstract void ApplyMonthEnd(); //it will be applied in program.cs

        public void PrintStatement()
        {
            Console.WriteLine($"Account Statement for {Owner} (ID: {Id})");
            Console.WriteLine("--------------------------------------------------");
            PrintRecords();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Current Balance: {Balance:C}");
        }

        public void PrintRecords()
        {
            foreach (var record in _records)
            {
                Console.WriteLine(record);
            }
        }
        protected bool NegativeBalanceCheck(decimal amount, decimal limit)
        {
            return (Balance - amount) < limit;
        }
    }
}
