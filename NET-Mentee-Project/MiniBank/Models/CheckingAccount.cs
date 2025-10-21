using MiniBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Models
{
    public class CheckingAccount : BankAccount, IOverdraftPolicy, IStatement, ITransactable
    {
        public decimal OverdraftLimit { get; }

        public CheckingAccount(int id, string owner, decimal initialBalance, decimal overdraftLimit =-200m)
            : base(id, owner, initialBalance)
        {
            OverdraftLimit = overdraftLimit;
            AddingMessage($"Overdraft limit set to {OverdraftLimit:C}");
        }

        protected override bool CanWithdraw(decimal amount, out string? error)
        {
            if(NegativeBalanceCheck(amount, OverdraftLimit))
            {
                error = $"Withdrawal would exceed overdraft limit of {OverdraftLimit:F2}.";
                return false;
            }
            error = null;
            return true;
        }

        public override void ApplyMonthEnd()
        {
            // no monthly interest for checking accounts
        }

    }
}
