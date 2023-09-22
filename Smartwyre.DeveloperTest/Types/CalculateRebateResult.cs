namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateResult
{

    public bool Success { get; private set; }
    public string Reason { get; private set; }

    public static CalculateRebateResult Failed(string reason) => new() { Success = false, Reason = reason };

    public static CalculateRebateResult Succeeded() => new() { Success = true };
}
