using Archangel.Tests.P2PLoan.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Archangel.Tests.P2PLoan.Servcices
{
    public class BestAggregatedRateFromLendersService : IBestAggregatedRateFromLendersService
    {
        private readonly IPotentialLenderIncomeCalculator _incomeCalculator;

        public BestAggregatedRateFromLendersService(IPotentialLenderIncomeCalculator incomeCalculator)
        {
            _incomeCalculator = incomeCalculator;
        }

        public double? GetAggregatedBestRate(IList<Lender> lenders, short loanAmount, short amountOfTransactionsPerYear, short amountOfYears)
        {
            if (lenders == null)
                throw new ArgumentNullException($"{nameof(lenders)} can't be null");
            if (loanAmount <= 0 || amountOfYears <= 0 || amountOfTransactionsPerYear <= 0)
                throw new ArgumentException($"{nameof(loanAmount)} or {nameof(amountOfYears)} or {nameof(amountOfTransactionsPerYear)} is invalid");

            var appropriateLenders = GetAppropriatelenders(lenders, loanAmount);
            if (appropriateLenders == null)
                return null;

            double effectiveIncome = GetEffectiveIncomeForlenders(appropriateLenders, loanAmount, amountOfTransactionsPerYear, amountOfYears);

            //((A/P)^(1/m*n)-1)*m*n
            var rate = (Math.Pow(effectiveIncome / loanAmount, (double)1 / (amountOfTransactionsPerYear * amountOfYears)) - 1)
                       * amountOfTransactionsPerYear;

            return rate;
        }

        private IList<Lender> GetAppropriatelenders(IList<Lender> lenders, short loanAmount)
        {
            var resultLenders = new List<Lender>();
            var orderedLenders = lenders.OrderBy(s => s.Rate).ThenByDescending(s => s.Amount).ToList();

            var needItAmount = loanAmount;
            foreach (var lender in orderedLenders)
            {
                needItAmount -= lender.Amount > (ulong)short.MaxValue ? short.MaxValue : (short)lender.Amount;
                resultLenders.Add(lender);
                if (needItAmount <= 0)
                    return resultLenders;
            }

            return null;
        }

        private double GetEffectiveIncomeForlenders(IList<Lender> lenders, short loanAmount, short amountOfTransactionsPerYear, short amountOfYears)
        {
            double effectiveIncomeSum = 0;
            var tempLoanAmount = (ulong)loanAmount;

            foreach (var lender in lenders)
            {
                if (tempLoanAmount == 0)
                    break;

                if (tempLoanAmount >= lender.Amount)
                {
                    var effectiveIncome = _incomeCalculator.GetPotentialEffectiveIncome(lender.Rate, lender.Amount,
                        amountOfTransactionsPerYear, amountOfYears);
                    effectiveIncomeSum += effectiveIncome;
                    tempLoanAmount -= lender.Amount;
                }
                else
                {
                    var effectiveIncome = _incomeCalculator.GetPotentialEffectiveIncome(lender.Rate, tempLoanAmount,
                        amountOfTransactionsPerYear, amountOfYears);
                    effectiveIncomeSum += effectiveIncome;
                    tempLoanAmount = 0;
                }
            }

            return effectiveIncomeSum;
        }
    }
}