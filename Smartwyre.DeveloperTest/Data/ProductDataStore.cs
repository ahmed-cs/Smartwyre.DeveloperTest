using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    private static List<Product> Products = new List<Product> {
        new Product{ Id=1, Identifier="P001",Price=10m,SupportedIncentives=SupportedIncentiveType.FixedRateRebate, Uom="kg" },
        new Product{ Id=2, Identifier="P002",Price=20m,SupportedIncentives=SupportedIncentiveType.FixedCashAmount, Uom="kg" },
        new Product{ Id=3, Identifier="P003",Price=30m,SupportedIncentives=SupportedIncentiveType.AmountPerUom, Uom="kg" }
    };
    public async Task<Product> GetProduct(string productIdentifier)
    {
        return await Task.FromResult(Products.Where(r => r.Identifier == productIdentifier.ToUpper()).FirstOrDefault());
    }
}
