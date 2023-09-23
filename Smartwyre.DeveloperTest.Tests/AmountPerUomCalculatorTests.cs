using FluentAssertions;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests
{

    public class AmountPerUomCalculatorTests
    {
        private readonly AmountPerUomCalculator _amountPerUomCalc = new();

        [Fact]
        public void UnsupportedTypeReturnsFalseForProduct()
        {
            var product = new Product()
            {
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount
            };

            var isSupported = _amountPerUomCalc.IncentiveTypeSupported(product);

            isSupported.Should().BeFalse();
        }

        [Fact]
        public void SupportedTypeReturnsTrueForProduct()
        {
            var product = new Product()
            {
                SupportedIncentives = SupportedIncentiveType.AmountPerUom
            };

            var isSupported = _amountPerUomCalc.IncentiveTypeSupported(product);

            isSupported.Should().BeTrue();
        }

        [Fact]
        public void InvalidRebateReturnsInvalid()
        {
            var rebate = new Rebate();
            var product = new Product();
            var amount = 1m;

            var isValid = _amountPerUomCalc.RebateIsValid(rebate, product, amount);

            isValid.Should().BeFalse();
        }

        [Fact]
        public void ValidRebateReturnsValid()
        {
            var rebate = new Rebate()
            {
                Amount = 1m
            };
            var product = new Product();
            var amount = 1m;

            var isValid = _amountPerUomCalc.RebateIsValid(rebate, product, amount);

            isValid.Should().BeTrue();
        }

        [Fact]
        public void CalculateReturnsAmount()
        {
            var rebate = new Rebate()
            {
                Amount = 3m
            };
            var product = new Product();
            var amount = 2m;

            var isValid = _amountPerUomCalc.CalculateAmount(rebate, product, amount);

            isValid.Should().Be(6m);
        }
    }
}