using FluentAssertions;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class FixedRateRebateCalculatorTests
{
    private readonly FixedRateRebateCalculator _fixedRateCalc = new();

    [Fact]
    public void UnsupportedTypeReturnsFalseForProduct()
    {
        var product = new Product()
        {
            SupportedIncentives = SupportedIncentiveType.AmountPerUom
        };

        var isSupported = _fixedRateCalc.IncentiveTypeSupported(product);

        isSupported.Should().BeFalse();
    }

    [Fact]
    public void SupportedTypeReturnsTrueForProduct()
    {
        var product = new Product()
        {
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate
        };

        var isSupported = _fixedRateCalc.IncentiveTypeSupported(product);

        isSupported.Should().BeTrue();
    }

    [Fact]
    public void InvalidRebateReturnsInvalid()
    {
        var rebate = new Rebate();
        var product = new Product();
        var amount = 1m;

        var isValid = _fixedRateCalc.RebateIsValid(rebate, product, amount);

        isValid.Should().BeFalse();
    }

    [Fact]
    public void ValidRebateReturnsValid()
    {
        var rebate = new Rebate()
        {
            Percentage = 1m
        };
        var product = new Product()
        {
            Price = 1m
        };
        var amount = 1m;

        var isValid = _fixedRateCalc.RebateIsValid(rebate, product, amount);

        isValid.Should().BeTrue();
    }

    [Fact]
    public void CalculateReturnsAmount()
    {
        var rebate = new Rebate()
        {
            Percentage = 5m
        };
        var product = new Product()
        {
            Price = 3m
        };
        var amount = 2m;

        var isValid = _fixedRateCalc.CalculateAmount(rebate, product, amount);

        isValid.Should().Be(30m);
    }
}