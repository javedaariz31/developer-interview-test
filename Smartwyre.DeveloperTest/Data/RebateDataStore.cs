using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{

    public Rebate GetRebate(string rebateIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return rebateIdentifier switch
        {
            nameof(IncentiveType.FixedRateRebate) => new Rebate()
            {
                Amount = 1,
                Identifier = nameof(IncentiveType.FixedRateRebate),
                Incentive = IncentiveType.FixedRateRebate,
                Percentage = 10m
            },
            nameof(IncentiveType.AmountPerUom) => new Rebate()
            {
                Amount = 1,
                Identifier = nameof(IncentiveType.AmountPerUom),
                Incentive = IncentiveType.AmountPerUom,
                Percentage = 10m
            },
            nameof(IncentiveType.FixedCashAmount) => new Rebate()
            {
                Amount = 1,
                Identifier = nameof(IncentiveType.FixedCashAmount),
                Incentive = IncentiveType.FixedCashAmount,
                Percentage = 10m
            },
            _ => null
        };
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
