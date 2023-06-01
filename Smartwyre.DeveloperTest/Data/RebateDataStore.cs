using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    private static List<Rebate> Rebates = new List<Rebate> {
        new Rebate{Amount=2m,Identifier="R001",Incentive=IncentiveType.FixedRateRebate,Percentage=2m},
        new Rebate{Amount=3m,Identifier="R002",Incentive=IncentiveType.FixedCashAmount,Percentage=4m},
        new Rebate{Amount=4m,Identifier="R003",Incentive=IncentiveType.AmountPerUom,Percentage=6m},
    };
    public async Task<Rebate> GetRebate(string rebateIdentifier)
    {
        return await Task.FromResult(Rebates.Where(r => r.Identifier == rebateIdentifier.ToUpper()).FirstOrDefault());
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
