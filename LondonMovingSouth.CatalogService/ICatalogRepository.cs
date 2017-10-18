using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LondonMovingSouth.CatalogService
{
    public interface ICatalogRepository
    {
        Task<bool> AddProductAsync(Product product);
        Task<bool> DeleteProductAsync(string name);
        Task<bool> UpdateProductAsync(Product product);
        Task<Product> GetProductAsync(string name);
        Task<IEnumerable<Product>> GetCatalogAsync(string count, string offset, DateTime? fromDate, DateTime? toDate);
    }
}