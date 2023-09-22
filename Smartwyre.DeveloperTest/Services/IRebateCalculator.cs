using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services
{
    public interface IRebateCalculator
    {
        bool IncentiveTypeSupported(Product product);
        bool RebateIsValid(Rebate rebate, Product product, decimal volume);
        decimal CalculateAmount(Rebate rebate, Product product, decimal volume);
    }
}