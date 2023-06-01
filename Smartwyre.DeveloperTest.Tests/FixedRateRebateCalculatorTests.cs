using Smartwyre.DeveloperTest.RebateCalculator;
using Smartwyre.DeveloperTest.Types;
using System;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

//*************** Please note that the similar sort of tests could be written for other implementation as well **************** 
public class FixedRateRebateCalculatorTests
{
    private Product product = new Product { Price = 10m,SupportedIncentives = SupportedIncentiveType.FixedRateRebate };
    private Rebate rebate = new Rebate { Percentage = 10m};
    private CalculateRebateRequest calculateRebateRequest = new CalculateRebateRequest { Volume = 10 };
    [Fact]
    public void WhenProductDoesNotSupportTheIncentive_SuccessShouldBeFalse()
    {
        product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;
        FixedRateRebateCalculator fixedRateRebateCalculator = new FixedRateRebateCalculator();
        var result = fixedRateRebateCalculator.Calculate(rebate, product, calculateRebateRequest);
        Assert.False(result.Success);
    }
    [Fact]
    public void WhenRebatePercentageIsZero_SuccessShouldBeFalse()
    {
        rebate.Percentage = 0;
        FixedRateRebateCalculator fixedRateRebateCalculator = new FixedRateRebateCalculator();
        var result = fixedRateRebateCalculator.Calculate(rebate, product, calculateRebateRequest);
        Assert.False(result.Success);
    }
    [Fact]
    public void WhenRebateProductPriceIsZero_SuccessShouldBeFalse()
    {
        product.Price = 0m;
        FixedRateRebateCalculator fixedRateRebateCalculator = new FixedRateRebateCalculator();
        var result = fixedRateRebateCalculator.Calculate(rebate, product, calculateRebateRequest);
        Assert.False(result.Success);
    }

    [Fact]
    public void WhenCalculateWithValidValues_SuccessShouldBeTrue_And_RebateAmountShouldBeCalculated()
    {
        FixedRateRebateCalculator fixedRateRebateCalculator = new FixedRateRebateCalculator();
        var result = fixedRateRebateCalculator.Calculate(rebate, product, calculateRebateRequest);
        Assert.True(result.Success);
        Assert.Equal(1000m, result.RebateAmount);
    }

}
