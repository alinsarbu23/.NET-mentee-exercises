using MiniBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Services
{
    public class AccountRegistry
    {
        private readonly List<BankAccount> _accounts = new();
        private int _nextId = 1;

        public IReadOnlyList<BankAccount> Accounts => _accounts;
        public int NextId() => _nextId++;

        public T Add<T>(T account) where T : BankAccount
        {
            if (account is null)
                throw new ArgumentNullException(nameof(account));

            _accounts.Add(account);
            return account;
        }

        public BankAccount? FindById(int id) => _accounts.FirstOrDefault(a => a.Id == id);

        public bool Transfer(int fromAccountId, int toAccountId, decimal amount, out string? error)
        {
            error = null;

            if (amount <= 0)
            {
                error = "Transfer amount must be positive.";
                return false;
            }

            var fromAcc = FindById(fromAccountId);
            var toAcc = FindById(toAccountId);

            if (fromAcc is null)
            {
                error = $"Source account with ID {fromAccountId} is not found.";
                return false;
            }
            if (toAcc is null)
            {
                error = $"Destination account with ID {toAccountId} is not found.";
                return false;
            }
            if (fromAcc == toAcc)
            {
                error = "Source and destination must be different.";
                return false;
            }

            if (!fromAcc.Withdraw(amount, out var withdrawErr))
            {
                error = $"Transfer failed on withdraw: {withdrawErr}";
                return false;
            }

            if (!toAcc.Deposit(amount, out var depositErr))
            {
                fromAcc.Deposit(amount, out _);
                error = $"Transfer failed on deposit: {depositErr}";
                return false;
            }

            return true;
        }


        public bool RemoveById(int id)
        {
            var acc = FindById(id);
            if (acc is null) return false;
            _accounts.Remove(acc);
            return true;
        }

    }
}
