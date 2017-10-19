using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LondonMovingSouth.CatalogService
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly string _connectionString;
        private readonly CatalogDbContext _dbContext;
        private readonly ILoggerFactory _loggerFactory;

        public CatalogRepository(string connectionString, CatalogDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _connectionString = connectionString;
            _dbContext = dbContext;
            _loggerFactory = loggerFactory;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            using (_dbContext)
            using (var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    const bool result = true;

                    await _dbContext.Products.AddAsync(product);
                    await _dbContext.SaveChangesAsync();
                    dbContextTransaction.Commit();

                    return result;
                }
                catch (Exception exception)
                {
                    
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            using (_dbContext)
            using (var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    const bool result = true;

                    var deleteProduct = await _dbContext.Products.FindAsync(id);
                    _dbContext.Products.Remove(deleteProduct);
                    await _dbContext.SaveChangesAsync();

                    dbContextTransaction.Commit();

                    return result;
                }
                catch (Exception exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            using (_dbContext)
            using (var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    const bool result = true;

                    var updateProduct = await _dbContext.Products.FindAsync(product.Id);

                    updateProduct.DetailId = product.DetailId;
                    updateProduct.Category = product.Category;
                    updateProduct.CreatedDate = product.CreatedDate;
                    updateProduct.ModifiedDate = product.ModifiedDate;
                    updateProduct.Price = product.Price;
                    updateProduct.Summary = product.Summary;

                    await _dbContext.SaveChangesAsync();
                    dbContextTransaction.Commit();

                    return result;
                }
                catch (Exception exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<Product> GetProductAsync(string name)
        {
            var result =  new Product() { Name = name, DateFormatted = DateTime.UtcNow.ToShortTimeString(), Summary = "summary", Price = 100.00M };
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
