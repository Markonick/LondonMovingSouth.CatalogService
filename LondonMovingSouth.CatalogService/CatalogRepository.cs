using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LondonMovingSouth.CatalogService
{
    public class CatalogRepository : ICatalogRepository
    {
        public async Task<bool> AddProductAsync(Product drink)
        {
            const bool result = true;
            await Task.Delay(5000);
            return result;
        }

        public async Task<bool> DeleteProductAsync(string name)
        {
            const bool result = true;
            await Task.Delay(5000);
            return result;
        }

        public async Task<bool> UpdateProductAsync(Product drink)
        {
            const bool result = true;
            await Task.Delay(5000);
            return result;
        }

        public async Task<Product> GetProductAsync(string name)
        {
            var result =  new Product() { Name = "name", DateFormatted = DateTime.UtcNow.ToShortTimeString(), Summary = "summary", Price = 100.00M };
            await Task.Delay(5000);
            return result;
        }

        public async Task<IEnumerable<Product>> GetCatalogAsync(string count, string offset, DateTime? fromDate, DateTime? toDate)
        {
            IEnumerable<Product> result = new List<Product>(){new Product(){Name = "name", DateFormatted = DateTime.UtcNow.ToShortTimeString(), Summary = "summary", Price = 100.00M}};
            await Task.Delay(5000);
            return result;
        }
    }
}
