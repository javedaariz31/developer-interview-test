using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    // Access database to retrieve account, code removed for brevity 
    public Product GetProduct(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return productIdentifier switch
        {
            nameof(SupportedIncentiveType.FixedRateRebate) => new Product()
            {
                Id = 1,
                Identifier = nameof(SupportedIncentiveType.FixedRateRebate),
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
                Uom = "Cubic Meter",
                Price = 100
            },
            nameof(SupportedIncentiveType.AmountPerUom) => new Product()
            {
                Id = 2,
                Identifier = nameof(SupportedIncentiveType.AmountPerUom),
                SupportedIncentives = SupportedIncentiveType.AmountPerUom,
                Uom = "Liters",
                Price = 200
            },
            nameof(SupportedIncentiveType.FixedCashAmount) => new Product()
            {
                Id = 3,
                Identifier = nameof(SupportedIncentiveType.FixedCashAmount),
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
                Uom = "Metric Ton",
                Price = 1000
            },
            _ => null
        };
    }
}
