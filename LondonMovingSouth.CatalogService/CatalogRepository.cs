using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace LondonMovingSouth.CatalogService
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public CatalogRepository(string connectionString, ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            using (var dbContext = new CatalogDbContext(_connectionString))
            using (var dbContextTransaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    const bool result = true;

                    await dbContext.Products.AddAsync(product);

                    await dbContext.SaveChangesAsync();

                    dbContextTransaction.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.Debug(ex.Message);
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            using (var dbContext = new CatalogDbContext(_connectionString))
            using (var dbContextTransaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    const bool result = true;

                    var deleteProduct = await dbContext.Products.FindAsync(id);
                    dbContext.Products.Remove(deleteProduct);

                    await dbContext.SaveChangesAsync();

                    dbContextTransaction.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.Debug(ex.Message);
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            using (var dbContext = new CatalogDbContext(_connectionString))
            using (var dbContextTransaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    const bool result = true;

                    var updateProduct = await dbContext.Products.FindAsync(product.Id);

                    updateProduct.DetailId = product.DetailId;
                    updateProduct.Category = product.Category;
                    updateProduct.CreatedDate = product.CreatedDate;
                    updateProduct.ModifiedDate = product.ModifiedDate;
                    updateProduct.Price = product.Price;
                    updateProduct.Summary = product.Summary;

                    await dbContext.SaveChangesAsync();

                    dbContextTransaction.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.Debug(ex.Message);
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<Product> GetProductAsync(int id)
        {
            using (var dbContext = new CatalogDbContext(_connectionString))
            {
                try
                {
                    return await dbContext.Products.FindAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.Debug(ex.Message);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Product>> GetCatalogAsync(string count, string offset, DateTime? fromDate, DateTime? toDate)
        {
            using (var dbContext = new CatalogDbContext(_connectionString))
            {
                try
                {
                    return await dbContext.Products.ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.Debug(ex.Message);
                    throw;
                }
            }
        }
    }
}
