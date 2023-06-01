using Smartwyre.DeveloperTest.Types;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Services;

public interface IRebateService
{
    Task<CalculateRebateResult> Calculate(CalculateRebateRequest request);
}
