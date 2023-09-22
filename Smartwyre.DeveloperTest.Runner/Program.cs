using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IRebateDataStore, RebateDataStore>();
        services.AddSingleton<IProductDataStore, ProductDataStore>();
        services.AddSingleton<IRebateCalculatorFactory, RebateCalculatorFactory>();
        services.AddSingleton<IRebateService, RebateService>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        var rebateService = serviceProvider.GetService<IRebateService>();

        var request = new CalculateRebateRequest();

        SelectRebate(ref request);

        SelectProduct(ref request);

        EnterVolume(ref request);

        var result = rebateService.Calculate(request);

        Console.WriteLine($"Success: {result.Success}");

        if (!result.Success)
        {
            Console.WriteLine($"Error reason: {result.Reason}");
        }

        Console.ReadKey();
    }

    private static void EnterVolume(ref CalculateRebateRequest request)
    {
        Console.WriteLine("Enter volume");

        var volume = 0m;

        while (!decimal.TryParse(Console.ReadLine(), out volume))
        {
            Console.Write("Please enter decimal value: ");
        }

        request.Volume = volume;
    }

    private static void SelectProduct(ref CalculateRebateRequest request)
    {
        Console.WriteLine("Select product: ");

        Console.WriteLine($"1: {nameof(IncentiveType.FixedRateRebate)}");
        Console.WriteLine($"2: {nameof(IncentiveType.AmountPerUom)}");
        Console.WriteLine($"3: {nameof(IncentiveType.FixedCashAmount)}");
        Console.WriteLine("4: Missing");
        Console.Write("Enter product option: ");
        var productChoice = 0;

        while (!int.TryParse(Console.ReadLine(), out productChoice))
        {
            Console.Write("Please enter valid entry : ");
        }

        request.ProductIdentifier = productChoice switch
        {
            1 => nameof(IncentiveType.FixedRateRebate),
            2 => nameof(IncentiveType.AmountPerUom),
            3 => nameof(IncentiveType.FixedCashAmount),
            _ => "Missing"
        };
        Console.WriteLine($"Selected product: {request.ProductIdentifier}");
    }

    private static void SelectRebate(ref CalculateRebateRequest request)
    {
        Console.WriteLine("Select rebate type: ");
        Console.WriteLine($"1: {nameof(IncentiveType.FixedRateRebate)}");
        Console.WriteLine($"2: {nameof(IncentiveType.AmountPerUom)}");
        Console.WriteLine($"3: {nameof(IncentiveType.FixedCashAmount)}");
        Console.WriteLine("4: Missing");
        Console.WriteLine("Enter rebate option: ");
        var rebateChoice = 0;

        while (!int.TryParse(Console.ReadLine(), out rebateChoice))
        {
            Console.Write("Please enter valid entry : ");
        }

        request.RebateIdentifier = rebateChoice switch
        {
            1 => nameof(IncentiveType.FixedRateRebate),
            2 => nameof(IncentiveType.AmountPerUom),
            3 => nameof(IncentiveType.FixedCashAmount),
            _ => "Missing"
        };
        Console.WriteLine($"Selected rebate type : {request.RebateIdentifier}");
    }
}