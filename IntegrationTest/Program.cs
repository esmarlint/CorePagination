using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PaginatorBenchmarks>();
        }
    }
}