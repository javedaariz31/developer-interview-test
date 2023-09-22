using FluentAssertions;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class FixedCashAmountCalculatorTests
{
    private readonly FixedCashAmountCalculator _fixedCashAmountCalc = new();

    [Fact]
    public void UnsupportedTypeReturnsFalseForProduct()
    {
        var product = new Product()
        {
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };

        var isSupported = _fixedCashAmountCalc.IncentiveTypeSupported(product);

        isSupported.Should().BeFalse();
    }

    [Fact]
    public void SupportedTypeReturnsTrueForProduct()
    {
        var product = new Product()
        {
            SupportedIncentives = SupportedIncentiveType.FixedCashAmount
        };

        var isSupported = _fixedCashAmountCalc.IncentiveTypeSupported(product);

        isSupported.Should().BeTrue();
    }

    [Fact]
    public void InvalidRebateReturnsInvalid()
    {
        var rebate = new Rebate();
        var product = new Product();
        var amount = 1m;

        var isValid = _fixedCashAmountCalc.RebateIsValid(rebate, product, amount);

        isValid.Should().BeFalse();
    }

    [Fact]
    public void ValidRebateReturnsValid()
    {
        var rebate = new Rebate()
        {
            Amount = 1
        };
        var product = new Product();
        var amount = 1m;

        var isValid = _fixedCashAmountCalc.RebateIsValid(rebate, product, amount);

        isValid.Should().BeTrue();
    }

    [Fact]
    public void CalculateReturnsAmount()
    {
        const decimal rebateAmount = 100m;
        var rebate = new Rebate()
        {
            Amount = rebateAmount
        };
        var product = new Product();
        var amount = 1m;

        var isValid = _fixedCashAmountCalc.CalculateAmount(rebate, product, amount);

        isValid.Should().Be(rebateAmount);
    }
}