using Archangel.Tests.P2PLoan.Models;
using Archangel.Tests.P2PLoan.Servcices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archangel.Tests.P2PLoan.Commands
{
    class GetBestLoanConditionCommand : IGetBestLoanConditionCommand
    {
        private const short AmountOfTransactionsPerYear = 12;
        private const short AmountOfYears = 3;

        private readonly ICsvHandler _csvHandler;
        private readonly IBestAggregatedRateFromLendersService _bestAggregatedRateFromLendersService;

        public GetBestLoanConditionCommand(ICsvHandler csvHandler, IBestAggregatedRateFromLendersService bestAggregatedRateFromLendersService)
        {
            _csvHandler = csvHandler;
            _bestAggregatedRateFromLendersService = bestAggregatedRateFromLendersService;
        }

        public async Task<CommandResult<BestLoanConditionModel>> ExecuteAsync(IList<string> csvs, short loanAmount)
        {
            IList<Lender> lenders;
            try
            {
                lenders = await _csvHandler.GetLendersAsync(csvs);
            }
            catch (Exception e)
            {
                return new CommandResult<BestLoanConditionModel>(false, e.Message);
            }


            if (loanAmount < 1000 || loanAmount > 15000 || loanAmount % 100 != 0)
                return new CommandResult<BestLoanConditionModel>(false, $"Requested amount {loanAmount} is invalid");

            var effectiveRate = _bestAggregatedRateFromLendersService.GetAggregatedBestRate(lenders, loanAmount, AmountOfTransactionsPerYear, AmountOfYears);
            if (effectiveRate == null)
                return new CommandResult<BestLoanConditionModel>(false, "Not enough sources for borrowing. Try lower amount");

            //((1 + rate)^(1/m)-1) * m
            var annualNominalRate = (Math.Pow(1 + effectiveRate.Value, (double)1 / AmountOfTransactionsPerYear) - 1) * AmountOfTransactionsPerYear;

            //pmt = P * (r(1+r)^(m^n))/((1+r)^(m*n)-1)
            var t = Math.Pow(1 + annualNominalRate / AmountOfTransactionsPerYear, AmountOfTransactionsPerYear * AmountOfYears);
            var pmt = loanAmount * (annualNominalRate / AmountOfTransactionsPerYear * t) / (t - 1);

            var totalLoan = pmt * AmountOfTransactionsPerYear * AmountOfYears;

            return new CommandResult<BestLoanConditionModel>(new BestLoanConditionModel(loanAmount, pmt, totalLoan, effectiveRate.Value * 100));
        }
    }
}
