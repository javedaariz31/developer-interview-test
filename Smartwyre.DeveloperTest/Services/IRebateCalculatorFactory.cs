using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public interface IRebateCalculatorFactory
{
    public IRebateCalculator Build(IncentiveType type);
}