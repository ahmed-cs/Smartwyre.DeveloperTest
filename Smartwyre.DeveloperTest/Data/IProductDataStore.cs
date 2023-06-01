using Smartwyre.DeveloperTest.Types;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Data
{
    public interface IProductDataStore
    {
        Task<Product> GetProduct(string productIdentifier);
    }
}