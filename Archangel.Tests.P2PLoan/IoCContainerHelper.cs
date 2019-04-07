using Archangel.Tests.P2PLoan.Commands;
using Archangel.Tests.P2PLoan.Servcices;
using SimpleInjector;

namespace Archangel.Tests.P2PLoan
{
    public static class IoCContainerHelper
    {
        public static void AddContainer(Container container)
        {
            container.AddServices();
            container.Register<IGetBestLoanConditionCommand, GetBestLoanConditionCommand>(Lifestyle.Transient);
        }
    }
}