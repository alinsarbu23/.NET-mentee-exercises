using MiniBank.Models;
using MiniBank.SerializeDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        public BankAccount? FindById(int id)
        {
            return _accounts.FirstOrDefault(a => a.Id == id);
        }

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

        public void SaveToJSON(string path)
        {
            var accounts = new ListBankAcountsDTO(
                Accounts: _accounts.Select(
                    a=> new BankAccountDTO(
                        Id: a.Id,
                        Owner: a.Owner,
                        Type: a.GetType().Name,
                        Balance: a.Balance,
                        Records: a.GetRecords()
                        )).ToList()
            );

            var opt = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(accounts, opt);
            File.WriteAllText(path, json);
        }

        public bool LoadFromJSON(string path, out string? error)
        {
            error = null;
            ListBankAcountsDTO? accountsDTO;
            if (!File.Exists(path))
            {
                error = "data.json file not found.";
                return false;
            }
            try
            {
                var json = File.ReadAllText(path);
                accountsDTO = JsonSerializer.Deserialize<ListBankAcountsDTO>(json);
                if (accountsDTO is null)
                {
                    error = $"Failed to deserialize {path}";
                    return false;
                }
            }

            catch (Exception ex)
            {
                error = $"Error loading from JSON: {ex.Message}";
                return false;
            }

            _accounts.Clear();

            foreach(var dto in accountsDTO.Accounts)
            {
                BankAccount account = dto.Type switch
                {
                    nameof(CheckingAccount) => new CheckingAccount(dto.Id, dto.Owner, dto.Balance),
                    nameof(SavingsAccount) => new SavingsAccount(dto.Id, dto.Owner, dto.Balance),
                    nameof(LoanAccount) => new LoanAccount(dto.Id, dto.Owner, dto.Balance)
                };

                account.RestoreState(dto.Balance, dto.Records);
                _accounts.Add(account);
            }
            _nextId = NextId();
            return true;

        }
    }
}
