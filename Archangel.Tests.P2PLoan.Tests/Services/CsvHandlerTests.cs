using System.Collections.Generic;
using Archangel.Tests.P2PLoan.Servcices;
using FluentAssertions;
using Xunit;

namespace Archangel.Tests.P2PLoan.Tests.Services
{
    public class CsvHandlerTests
    {
        private CsvHandler service;

        public CsvHandlerTests()
        {
            service = new CsvHandler();
        }

        [Fact]
        public async void GetLendersAsync_AllDataIsCorrect_return2Lenders()
        {
            var list = new List<string>
            {
                "name,rate,amount",
                "bob,0.01,1000",
                "Anna,0.02,500"
            };
            var result = await service.GetLendersAsync(list);
            result.Count.Should().Be(2);
        }

        [Theory]
        [InlineData("-10")]
        [InlineData("0.10")]
        [InlineData("18446744073709551616")]
        public async void GetLendersAsync_InvalidAmmount_Return1Lender(string amount)
        {
            var list = new List<string>
            {
                "name,rate,amount",
                $"bob,0.01,{amount}",
                "Anna,0.02,500"
            };
            var result = await service.GetLendersAsync(list);
            result.Count.Should().Be(1);
            result[0].Name.Should().Be("Anna");
        }

        [Theory]
        [InlineData("-10")]
        public async void GetLendersAsync_InvalidRate_ReturnOneLender(string rate)
        {
            var list = new List<string>
            {
                "name,rate,amount",
                $"bob,{rate},1000",
                "Anna,0.02,500"
            };
            var result = await service.GetLendersAsync(list);
            result.Count.Should().Be(1);
            result[0].Name.Should().Be("Anna");
        }

        [Theory]
        [InlineData("0.01,bob,1000")]
        [InlineData("0.01,1000,bob")]
        [InlineData("bob,0.01,")]
        [InlineData("0.01,bob")]
        [InlineData("0.01,bob,1000,")]
        [InlineData("0.01,bob,1000,Bob")]
        public async void GetLendersAsync_InvalidCombinationsInLine_NoLenders(string data)
        {
            var list = new List<string>
            {
                data
            };
            var result = await service.GetLendersAsync(list);
            result.Count.Should().Be(0);
        }
    }
}