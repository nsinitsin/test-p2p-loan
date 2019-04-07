using System;
using System.Collections.Generic;
using System.Text;

namespace Archangel.Tests.P2PLoan.Models
{
    public class Lender
    {
        public Lender(string name, double rate, ulong amount)
        {
            if (string.IsNullOrEmpty(name))
                throw  new ArgumentException(nameof(name));
            if (rate < 0)
                throw new ArgumentException($"{rate} can't store less then 0");
            if (amount < 0)
                throw new ArgumentException($"{rate} can't store less then 0");

            Name = name;
            Rate = rate;
            Amount = amount;
        }

        public string Name { get; }

        public double Rate { get; }

        public ulong Amount { get; }
    }
}
