using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.RebateCalculator
{
    public class FixedRateRebateCalculator : IRebateCalculator
    {
        public CalculateRebateResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            var result = new CalculateRebateResult();
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) || rebate.Percentage == 0 ||
                product.Price == 0)
            {
                result.Success = false;
            }
            else
            {
                result.RebateAmount += product.Price * rebate.Percentage * request.Volume;
                result.Success = true;
            }
            return result;
        }
        public bool GetCalculatorType(IncentiveType incentiveType) => incentiveType == IncentiveType.FixedRateRebate;
    }
}
