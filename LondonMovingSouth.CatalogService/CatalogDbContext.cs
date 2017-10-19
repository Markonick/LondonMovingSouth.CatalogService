using Microsoft.EntityFrameworkCore;

namespace LondonMovingSouth.CatalogService
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Details> Details { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Details>().ToTable("Details");
        }
    }
}
