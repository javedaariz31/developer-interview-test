using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class FixedCashAmountCalculator : IRebateCalculator
{
    public bool IncentiveTypeSupported(Product product)
    {
        return IncentiveType.FixedCashAmount switch
        {
            IncentiveType.FixedRateRebate => product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate),
            IncentiveType.AmountPerUom => product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom),
            IncentiveType.FixedCashAmount => product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount),
            _ => false
        };
    }

    public bool RebateIsValid(Rebate rebate, Product product, decimal volume)
    {
        return rebate.Amount != 0;
    }

    public decimal CalculateAmount(Rebate rebate, Product product, decimal volume)
    {
        return rebate.Amount;
    }
}