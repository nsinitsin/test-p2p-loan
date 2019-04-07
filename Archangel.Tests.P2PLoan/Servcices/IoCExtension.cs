using SimpleInjector;

namespace Archangel.Tests.P2PLoan.Servcices
{
    static class IoCExtension
    {
        public static void AddServices(this Container container)
        {
            container.Register<IPotentialLenderIncomeCalculator, PotentialLenderIncomeCalculator>(Lifestyle.Transient);
            container.Register<ICsvHandler, CsvHandler>(Lifestyle.Transient);
            container.Register<IBestAggregatedRateFromLendersService, BestAggregatedRateFromLendersService>(Lifestyle.Transient);
        }
    }
}
