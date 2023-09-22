using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateCalculatorFactory : IRebateCalculatorFactory
{
    public IRebateCalculator Build(IncentiveType type)
    {
        return type switch
        {
            IncentiveType.FixedRateRebate => new FixedRateRebateCalculator(),
            IncentiveType.AmountPerUom => new AmountPerUomCalculator(),
            IncentiveType.FixedCashAmount => new FixedCashAmountCalculator(),
            _ => null
        };
    }
}