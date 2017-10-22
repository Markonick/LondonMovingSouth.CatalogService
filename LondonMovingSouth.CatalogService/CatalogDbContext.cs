using Microsoft.EntityFrameworkCore;

namespace LondonMovingSouth.CatalogService
{
    public class CatalogDbContext : DbContext, ICatalogDbContext
    {
        private readonly string _connectionString;
        public DbSet<Product> Products { get; set; }
        public DbSet<Details> Details { get; set; }

        public CatalogDbContext() { }

        public CatalogDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LondonMovingSouthDb;Trusted_Connection=True;MultipleActiveResultSets=true");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Details>().ToTable("Details");
        }
    }
}
