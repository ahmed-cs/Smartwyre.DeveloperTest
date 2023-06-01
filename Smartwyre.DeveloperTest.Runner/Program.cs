using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.RebateCalculator;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static async Task Main(string[] args)
    {

        var host = RegisterServices(args);

        Console.Write("Enter Product Identifier from P001,P002,P003: ");
        string productIdentifier = Console.ReadLine();
        ValidateInput(productIdentifier);
        Console.Write("Enter Rebate Identifier from R001,R002,R003: ");
        string rebateIdentifier = Console.ReadLine();
        ValidateInput(rebateIdentifier);
        Console.Write("Enter Volumn in decimal: ");
        decimal volume = 0m;
        decimal.TryParse(Console.ReadLine(), out volume);
        if (volume == 0)
        {
            Console.WriteLine("Provided volume is not valid");
            Console.ReadKey();
            Environment.Exit(0);
        }
        

        CalculateRebateRequest request = new CalculateRebateRequest
        {
            ProductIdentifier = productIdentifier,
            RebateIdentifier = rebateIdentifier,
            Volume = volume
        };

        var rebateResult = await host.Services.GetRequiredService<IRebateService>().Calculate(request);

        if (rebateResult.Success)
            Console.WriteLine($"Rebate Amount: {rebateResult.RebateAmount}");
        else
            Console.WriteLine($"No Rebate Available");

        Console.ReadKey();
    }
    private static IHost RegisterServices(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddTransient<IRebateCalculator, AmountPerUomCalculator>();
                services.AddTransient<IRebateCalculator, FixedCashAmountCalculator>();
                services.AddTransient<IRebateCalculator, FixedRateRebateCalculator>();
                services.AddTransient<IRebateDataStore, RebateDataStore>();
                services.AddTransient<IProductDataStore, ProductDataStore>();
                services.AddTransient<IRebateService, RebateService>();
            })
        .Build();
    }
    private static void ValidateInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Bad Input");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
