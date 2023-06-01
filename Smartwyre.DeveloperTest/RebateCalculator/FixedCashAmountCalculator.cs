using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.RebateCalculator
{
    public class FixedCashAmountCalculator : IRebateCalculator
    {
        public CalculateRebateResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            var result = new CalculateRebateResult();
            if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) || rebate.Amount == 0)
            {
                result.Success = false;
            }
            else
            {
                result.RebateAmount = rebate.Amount;
                result.Success = true;
            }
            return result;
        }
        public bool GetCalculatorType(IncentiveType incentiveType) => incentiveType == IncentiveType.FixedCashAmount;
    }
}
