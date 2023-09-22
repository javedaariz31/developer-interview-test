using FluentAssertions;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateCalculatorFactoryTests
{
    private readonly RebateCalculatorFactory _rebateCalc = new();

    [Fact]
    public void BuildFixedRateRebate_ReturnsCalculator()
    {
        var calculator = _rebateCalc.Build(IncentiveType.FixedRateRebate);

        calculator.Should().NotBeNull().And.BeOfType<FixedRateRebateCalculator>();
    }

    [Fact]
    public void BuildAmountPerUom_ReturnsCalculator()
    {
        var calculator = _rebateCalc.Build(IncentiveType.AmountPerUom);

        calculator.Should().NotBeNull().And.BeOfType<AmountPerUomCalculator>();
    }

    [Fact]
    public void BuildFixedCashAmount_ReturnsCalculator()
    {
        var calculator = _rebateCalc.Build(IncentiveType.FixedCashAmount);

        calculator.Should().NotBeNull().And.BeOfType<FixedCashAmountCalculator>();
    }
}