using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archangel.Tests.P2PLoan.Models;

namespace Archangel.Tests.P2PLoan.Servcices
{
    public class CsvHandler : ICsvHandler
    {
        private const short HandleThreads = 4;

        public async Task<IList<Lender>> GetLendersAsync(IList<string> csvStrings)
        {
            var resultLenders = new List<Lender>();

            if (csvStrings.Count >= int.MaxValue)
                throw new Exception("Not supported amount of elements in csv");

            var amountOfThread = csvStrings.Count < HandleThreads ? csvStrings.Count : HandleThreads;
            var amountForTaken = (int)Math.Ceiling((double)csvStrings.Count / amountOfThread);
            var tasks = new List<Task<IList<Lender>>>(amountOfThread);

            for (int i = 0; i < amountOfThread; i++)
            {
                var groupCsv = csvStrings.Skip(i * amountForTaken).Take(amountForTaken);
                tasks.Add(Task.Factory.StartNew<IList<Lender>>(() => HandleCsvs(groupCsv)));
            }

            await Task.WhenAll(tasks.ToArray());

            foreach (var task in tasks)
            {
                var lenders = await task;
                resultLenders.AddRange(lenders);
            }

            return resultLenders;
        }

        private IList<Lender> HandleCsvs(IEnumerable<string> groupCsv)
        {
            var threadLendersList = new List<Lender>();

            foreach (var csvItem in groupCsv)
            {
                var csvArray = csvItem.Split(',');
                if (csvArray.Length != 3)
                    continue;

                var lenderName = csvArray[0];
                var rateStr = csvArray[1];
                var amountStr = csvArray[2];

                if (!ulong.TryParse(amountStr, out var amount))
                    continue;
                if (!double.TryParse(rateStr, out var rate))
                    continue;
                if (rate < 0)
                    continue;

                threadLendersList.Add(new Lender(lenderName, rate, amount));
            }

            return threadLendersList;
        }
    }
}