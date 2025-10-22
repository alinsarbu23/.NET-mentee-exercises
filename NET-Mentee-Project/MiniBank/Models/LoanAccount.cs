using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniBank.Models.Interfaces;

namespace MiniBank.Models
{
    public class LoanAccount : BankAccount, IInterestBearing, IStatement, ITransactable
    {
        public decimal MonthlyInterestRate { get; }

        public LoanAccount(int id, string owner, decimal initialLoanAmount, decimal monthlyInterestRate = 0.02m)
            : base(id, owner, -Math.Abs(initialLoanAmount))
        {
            MonthlyInterestRate = monthlyInterestRate;
            AddingMessage($"Loan created: {initialLoanAmount:C}; monthly interest {MonthlyInterestRate:P}");
        }


        protected override bool CanWithdraw(decimal amount, out string? error)
        {
            error = null;
            return true;
        }

        public void ApplyMonthlyInterest()
        {
            if (Balance < 0)
            {
                var interest = (-Balance) * MonthlyInterestRate; 
                Balance -= interest;
                AddingMessage($"Monthly debt interest -{interest:F2}. New balance {Balance:F2}");
            }
        }

        public override void ApplyMonthEnd() => ApplyMonthlyInterest();
    }
}
