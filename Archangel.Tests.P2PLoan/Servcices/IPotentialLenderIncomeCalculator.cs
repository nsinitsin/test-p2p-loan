namespace Archangel.Tests.P2PLoan.Servcices
{
    public interface IPotentialLenderIncomeCalculator
    {
        double GetPotentialEffectiveIncome(double rate, ulong amount, short amountOfTransactionsPerYear, short amountOfYears);
    }
}