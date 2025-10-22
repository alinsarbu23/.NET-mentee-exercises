using MiniBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Models
{
    public class SavingsAccount : BankAccount, IInterestBearing, IStatement, ITransactable
    {
        public decimal MonthlyInterestRate { get; } 
        public SavingsAccount(int id, string owner, decimal initialBalance, decimal monthlyInterestRate = 0.01m)
            : base(id, owner, initialBalance)
        {
            MonthlyInterestRate = monthlyInterestRate;
            AddingMessage($"Monthly interest rate set to {MonthlyInterestRate:P}");
        }
        protected override bool CanWithdraw(decimal amount, out string? error)
        {
            if(NegativeBalanceCheck(amount, 0m))
            {
                error = "Insufficient funds.";
                return false;
            }
            error = null;
            return true;
        }

        public void ApplyMonthlyInterest()
        {
            if (Balance <= 0)
            {
                return;
            }

            var interest = Balance * MonthlyInterestRate;
            Balance += interest;
            AddingMessage($"Monthly interest {interest:C}. The new balance is {Balance:C}");
        }

        public override void ApplyMonthEnd()
        {
            ApplyMonthlyInterest();
        }
    }
}
