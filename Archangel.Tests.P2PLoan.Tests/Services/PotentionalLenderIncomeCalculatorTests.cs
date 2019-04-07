using System;
using System.Collections.Generic;
using System.Text;
using Archangel.Tests.P2PLoan.Servcices;
using FluentAssertions;
using Xunit;

namespace Archangel.Tests.P2PLoan.Tests.Services
{
    public class PotentionalLenderIncomeCalculatorTests
    {
        [Fact]
        public void GetPotentionalIncome_Amount480Rate0069Year3Transactions12_590dot04()
        {
            var service = new PotentialLenderIncomeCalculator();
            var result = Math.Round(service.GetPotentialEffectiveIncome(0.069, 480, 12, 3), 2);
            result.Should().Be(590.04);
        }
    }
}
