using System.Collections.Generic;
using System.Threading.Tasks;
using Archangel.Tests.P2PLoan.Models;

namespace Archangel.Tests.P2PLoan.Commands
{
    internal interface IGetBestLoanConditionCommand
    {
        Task<CommandResult<BestLoanConditionModel>> ExecuteAsync(IList<string> csvs, short loanAmount);
    }
}