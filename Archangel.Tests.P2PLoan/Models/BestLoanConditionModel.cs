namespace Archangel.Tests.P2PLoan.Models
{
    public class BestLoanConditionModel
    {
        public BestLoanConditionModel(double requestedAmount, double monthlyRepayment, double totalRepayment, double rate)
        {
            RequestedAmount = requestedAmount;
            Rate = rate;
            MonthlyRepayment = monthlyRepayment;
            TotalRepayment = totalRepayment;
        }

        public double Rate { get; set; }

        public double TotalRepayment { get; set; }

        public double MonthlyRepayment { get; set; }

        public double RequestedAmount { get; set; }
    }
}