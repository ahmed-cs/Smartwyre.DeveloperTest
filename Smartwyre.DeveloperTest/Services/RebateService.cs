using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.RebateCalculator;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IEnumerable<IRebateCalculator> _rebateCalculators;
    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore, IEnumerable<IRebateCalculator> rebateCalculators)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _rebateCalculators = rebateCalculators;
    }
    public async Task<CalculateRebateResult> Calculate(CalculateRebateRequest request)
    {
        var rebateResult = new CalculateRebateResult();
        Rebate rebate = await _rebateDataStore.GetRebate(request.RebateIdentifier);
        if (rebate == null)
        { rebateResult.Success = false; return rebateResult; };
        Product product = await _productDataStore.GetProduct(request.ProductIdentifier);
        if (product == null)
        { rebateResult.Success = false; return rebateResult; };
        var calculator = _rebateCalculators.Where(calc => calc.GetCalculatorType(rebate.Incentive)).FirstOrDefault();
        if (calculator == null)
            throw (new Exception("Bad Incentive Type"));
        rebateResult = calculator.Calculate(rebate, product, request);
        if (rebateResult.Success)
        {
            _rebateDataStore.StoreCalculationResult(rebate, rebateResult.RebateAmount);
        }
        return rebateResult;
    }
}
