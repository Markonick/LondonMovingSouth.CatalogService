using Microsoft.EntityFrameworkCore;

namespace LondonMovingSouth.CatalogService
{
    public interface ICatalogDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Details> Details { get; set; }
    }
}