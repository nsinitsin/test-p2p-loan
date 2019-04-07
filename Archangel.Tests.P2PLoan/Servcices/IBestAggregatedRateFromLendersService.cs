using System.Collections.Generic;
using Archangel.Tests.P2PLoan.Models;

namespace Archangel.Tests.P2PLoan.Servcices
{
    public interface IBestAggregatedRateFromLendersService
    {
        double? GetAggregatedBestRate(IList<Lender> lenders, short loanAmount, short amountOfTransactionsPerYear,
            short amountOfYears);
    }
}