using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public class FixedRateRebateCalculator : IRebateCalculator
    {
        public bool IncentiveTypeSupported(Product product)
        {
            return IncentiveType.FixedRateRebate switch
            {
                IncentiveType.FixedRateRebate => product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate),
                IncentiveType.AmountPerUom => product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom),
                IncentiveType.FixedCashAmount => product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount),
                _ => false
            };
        }

        public bool RebateIsValid(Rebate rebate, Product product, decimal volume)
        {
            return 0 < rebate.Percentage && 0 < product.Price && 0 < volume;
        }

        public decimal CalculateAmount(Rebate rebate, Product product, decimal volume)
        {
            return rebate.Percentage * product.Price * volume;
        }
    }
}