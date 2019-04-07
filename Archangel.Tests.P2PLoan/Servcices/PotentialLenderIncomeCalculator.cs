using System;
using System.Collections.Generic;
using System.Text;

namespace Archangel.Tests.P2PLoan.Servcices
{
    public class PotentialLenderIncomeCalculator : IPotentialLenderIncomeCalculator
    {
        public double GetPotentialEffectiveIncome(double rate, ulong amount, short amountOfTransactionsPerYear, short amountOfYears)
        {
            //P*(1+rate/n)^(n*m)
            var value = (1 + rate / 12);
            var power = amountOfTransactionsPerYear * amountOfYears;


            var result = amount * Math.Pow(value, power);

            return result;
        }
    }
}
