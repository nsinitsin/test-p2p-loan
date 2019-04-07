using Archangel.Tests.P2PLoan.Commands;
using Archangel.Tests.P2PLoan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Archangel.Tests.P2PLoan
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Wrong amount of parameters.");
                Console.ReadKey();
                return;
            }

            var fileName = args[0];
            var loanAmountStr = args[1];

            if (!short.TryParse(loanAmountStr, out var loanAmount))
            {
                Console.WriteLine("Loan amount is invalid");
                Console.ReadKey();
                return;
            }

            var csvString = ReadCsvFile(fileName);
            if (csvString == null)
            {
                Console.WriteLine($"Can't read file {fileName} correctly");
                Console.ReadKey();
                return;
            }

            MainAsync(csvString, loanAmount).GetAwaiter().GetResult();
            
            Console.ReadKey();
        }


        static async Task MainAsync(IList<string> csvString, short loanAmount)
        {
            var iocContainer = new SimpleInjector.Container();
            IoCContainerHelper.AddContainer(iocContainer);

            var command = iocContainer.GetInstance<IGetBestLoanConditionCommand>();
            try
            {
                var commandResult = await command.ExecuteAsync(csvString, loanAmount);

                if (!commandResult.IsSuccess)
                {
                    Console.WriteLine(commandResult.Error);
                    return;
                }

                Console.WriteLine($"Requested amount: £{loanAmount}");
                Console.WriteLine($"Rate: {$"{Math.Round(commandResult.Result.Rate, 1):0.0}"}%");
                Console.WriteLine($"Monthly repayment: £{Math.Round(commandResult.Result.MonthlyRepayment, 2):0.00}");
                Console.WriteLine($"Total repayment: £{Math.Round(commandResult.Result.TotalRepayment, 2):0.00}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled exception. Message: {e}");
            }
        }

        private static IList<string> ReadCsvFile(string fileName)
        {
            try
            {
                return File.ReadAllLines(fileName);
            }
            catch
            {
                return null;
            }
        }
    }
}
