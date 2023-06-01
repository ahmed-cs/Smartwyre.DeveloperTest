using System;
using Xunit;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.RebateCalculator;
using System.Collections.Generic;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Tests.Stub;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    private readonly Mock<IRebateDataStore> _rebateDataStore;
    private readonly Mock<IProductDataStore> _productDataStore;
    private readonly List<IRebateCalculator> _rebateCalculators;
    private readonly Mock<IRebateCalculator> _calculator;
    public RebateServiceTests()
    {
        _rebateCalculators = new List<IRebateCalculator>();
        _rebateDataStore = new Mock<IRebateDataStore>();
        _productDataStore = new Mock<IProductDataStore>();
        _calculator = new Mock<IRebateCalculator>();
        _rebateCalculators.Add(_calculator.Object);
        _rebateDataStore.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(Task.FromResult(new Rebate { Incentive = IncentiveType.FixedRateRebate }));
        _productDataStore.Setup(r => r.GetProduct(It.IsAny<string>())).Returns(Task.FromResult(new Product()));
        _calculator.Setup(r => r.GetCalculatorType(It.IsAny<IncentiveType>())).Returns(true);
    }

    [Fact]
    public async void WhenCalculateWithInvalidRebate_ShouldNotCalculateRebate_And_NotStoreCalculationToDatabase()
    {
        _rebateDataStore.Setup(r => r.GetRebate(It.IsAny<string>())).Returns(Task.FromResult<Rebate>(null));
        var request = new CalculateRebateRequest { };
        RebateService rebateService = new RebateService(_rebateDataStore.Object, _productDataStore.Object, _rebateCalculators);
        var result = await rebateService.Calculate(request);
        Assert.False(result.Success);
        _calculator.Verify(r => r.Calculate(It.IsAny<Rebate>(), It.IsAny<Product>(), It.IsAny<CalculateRebateRequest>()), Times.Never());
        _rebateDataStore.Verify(r => r.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never());
    }
    [Fact]
    public async void WhenCalculateWithInvalidProduct_ShouldNotCalculateRebate_And_NotStoreCalculationToDatabase()
    {
        _productDataStore.Setup(r => r.GetProduct(It.IsAny<string>())).Returns(Task.FromResult<Product>(null));
        var request = new CalculateRebateRequest { };
        RebateService rebateService = new RebateService(_rebateDataStore.Object, _productDataStore.Object, _rebateCalculators);
        var result = await rebateService.Calculate(request);
        Assert.False(result.Success);
        _calculator.Verify(r => r.Calculate(It.IsAny<Rebate>(), It.IsAny<Product>(), It.IsAny<CalculateRebateRequest>()), Times.Never());
        _rebateDataStore.Verify(r => r.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never());
    }
    [Fact]
    public async void WhenCalculateWithValidIncentiveType_ShouldThrowException_And_NotStoreCalculationToDatabase()
    {
        _calculator.Setup(r => r.GetCalculatorType(It.IsAny<IncentiveType>())).Returns(false);
        var request = new CalculateRebateRequest { };
        RebateService rebateService = new RebateService(_rebateDataStore.Object, _productDataStore.Object, _rebateCalculators);
        var exception = await Assert.ThrowsAsync<Exception>(() => rebateService.Calculate(request));
        Assert.Equal("Bad Incentive Type", exception.Message);
        _calculator.Verify(r => r.Calculate(It.IsAny<Rebate>(), It.IsAny<Product>(), It.IsAny<CalculateRebateRequest>()), Times.Never());
        _rebateDataStore.Verify(r => r.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never());
    }
    [Fact]
    public async void WhenCalculateWithValidIncentiveType_ShouldCalculateWithCorrectCalculator()
    {
        var request = new CalculateRebateRequest { };
        var _mockCalculator = new List<IRebateCalculator>() { new MockCalculator() };
        RebateService rebateService = new RebateService(_rebateDataStore.Object, _productDataStore.Object, _mockCalculator);
        var result = await rebateService.Calculate(request);
        Assert.True(result.Success);
        Assert.Equal(10m, result.RebateAmount);
    }
    [Fact]
    public async void WhenCalculateWithValidProductAndRebate_ShouldCalculateRebateResult_And_StoreCalculationToDatabase()
    {
        _calculator.Setup(r => r.Calculate(It.IsAny<Rebate>(), It.IsAny<Product>(), It.IsAny<CalculateRebateRequest>())).Returns(new CalculateRebateResult { Success = true });
        var request = new CalculateRebateRequest { };
        RebateService rebateService = new RebateService(_rebateDataStore.Object, _productDataStore.Object, _rebateCalculators);
        var result = await rebateService.Calculate(request);
        _calculator.Verify(r => r.Calculate(It.IsAny<Rebate>(), It.IsAny<Product>(), It.IsAny<CalculateRebateRequest>()), Times.Once());
        _rebateDataStore.Verify(r => r.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Once());
    }

}
