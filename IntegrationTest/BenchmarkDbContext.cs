using Microsoft.EntityFrameworkCore;

namespace CorePagination.Benchmarks
{
    public class BenchmarkDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("BenchmarkDatabase");
        }
    }
}