using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LondonMovingSouth.CatalogService
{
    public interface ICatalogRepository
    {
        Task<bool> AddProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> UpdateProductAsync(Product product);
        Task<Product> GetProductAsync(int id);
        Task<IEnumerable<Product>> GetCatalogAsync(string count, string offset, DateTime? fromDate, DateTime? toDate);
    }
}