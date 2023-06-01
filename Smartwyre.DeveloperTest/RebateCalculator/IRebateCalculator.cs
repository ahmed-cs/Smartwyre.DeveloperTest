using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.RebateCalculator
{
    public interface IRebateCalculator
    {
        CalculateRebateResult Calculate(Rebate rebate, Product product, CalculateRebateRequest request);
        bool GetCalculatorType(IncentiveType incentiveType);
    }
}
