using Smartwyre.DeveloperTest.RebateCalculator;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Tests.Stub
{
    public class MockCalculator: IRebateCalculator
    {
        public CalculateRebateResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request)
        {
            return new CalculateRebateResult { Success = true, RebateAmount = 10m };
        }
        public bool GetCalculatorType(IncentiveType incentiveType) { return true; }
    }
}
