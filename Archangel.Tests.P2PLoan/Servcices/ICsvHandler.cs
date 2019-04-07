using System.Collections.Generic;
using System.Threading.Tasks;
using Archangel.Tests.P2PLoan.Models;

namespace Archangel.Tests.P2PLoan.Servcices
{
    public interface ICsvHandler
    {
        Task<IList<Lender>> GetLendersAsync(IList<string> csvStrings);
    }
}