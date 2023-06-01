using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.RebateCalculator
{
    public class AmountPerUomCalculator : IRebateCalculator
    {
        public CalculateRebateResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            var result = new CalculateRebateResult();
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) || rebate.Amount == 0)
            {
                result.Success = false;
            }
            else
            {
                result.RebateAmount += rebate.Amount * request.Volume;
                result.Success = true;
            }
            return result;
        }
        public bool GetCalculatorType(IncentiveType incentiveType) => incentiveType == IncentiveType.AmountPerUom;
    }
}
