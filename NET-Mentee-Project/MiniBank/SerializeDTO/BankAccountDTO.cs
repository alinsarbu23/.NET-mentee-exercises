using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.SerializeDTO
{
    public record BankAccountDTO(int Id, string Owner, string Type, decimal Balance, List<string> Records);
    public record ListBankAcountsDTO(List<BankAccountDTO> Accounts);
}
