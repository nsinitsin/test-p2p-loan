using System;
using System.Collections.Generic;
using Archangel.Tests.P2PLoan.Models;
using Archangel.Tests.P2PLoan.Servcices;
using FluentAssertions;
using Moq;
using Xunit;

namespace Archangel.Tests.P2PLoan.Tests.Services
{
    public class BestAggregatedRateFromLendersServiceTests
    {
        private readonly Mock<IPotentialLenderIncomeCalculator> _potentialCalculatorMock;
        private readonly BestAggregatedRateFromLendersService _service;

        private readonly IList<Lender> _lenders;

        public BestAggregatedRateFromLendersServiceTests()
        {
            _lenders = new List<Lender>
            {
                new Lender("Bob",0.075, 640),
                new Lender("Jane",0.069, 480),
                new Lender("Fred",0.071, 520),
                new Lender("Mary",0.104, 170),
                new Lender("John",0.081, 320),
                new Lender("Dave",0.074, 140),
                new Lender("Angela",0.071, 60),
            };

            _potentialCalculatorMock = new Mock<IPotentialLenderIncomeCalculator>();
            _potentialCalculatorMock.Setup(s =>
                    s.GetPotentialEffectiveIncome(It.IsAny<double>(), It.IsAny<ulong>(), It.IsAny<short>(),
                        It.IsAny<short>()))
                .Returns((double rate, ulong amount, short periods, short years) =>
                {
                    if (amount == 480)
                        return 590.04172245287;
                    return 643.03628992083;
                });

            _service = new BestAggregatedRateFromLendersService(_potentialCalculatorMock.Object);
        }

        [Fact]
        public void GetAggregatedBestRate_LendersIsNull_ThrownException()
        {
            Action act = () => _service.GetAggregatedBestRate(null, 1000, 12, 3);
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetAggregatedBestRate_LoanAmountIsInvalid_ThrownException(short loanAmount)
        {
            Action act = () => _service.GetAggregatedBestRate(_lenders, loanAmount, 12, 3);
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetAggregatedBestRate_amountOfYearsIsInvalid_ThrownException(short amountOfYears)
        {
            Action act = () => _service.GetAggregatedBestRate(_lenders, 1000, 12, amountOfYears);
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetAggregatedBestRate_AmountOfTransactionsPerYearIsInvalid_ThrownException(short periods)
        {
            Action act = () => _service.GetAggregatedBestRate(_lenders, 1000, periods, 3);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetAggregatedBestRate_NotEnoughMoneyAmount1000_Null()
        {
           var result =  _service.GetAggregatedBestRate(_lenders, 15000, 12, 3);
           result.Should().BeNull();
        }

        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData(1)]
        public void GetAggregatedBestRate_EdgeAmount_NotNullValue(short amount)
        {
            _lenders.Add(new Lender("Test", 0.084, 50000));
            var result = _service.GetAggregatedBestRate(_lenders, amount, 12, 3);
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetAggregatedBestRate_AllDataIsCorrect_0dot07()
        {
            var checkRate = 0.07;

            var result = Math.Round(_service.GetAggregatedBestRate(_lenders, 1000, 12, 3).Value, 2);
            result.Should().Be(checkRate);
        }
    }
}