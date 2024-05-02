using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CorePagination.Extensions;
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

    [MemoryDiagnoser]
    public class PaginationBenchmarks
    {
        private BenchmarkDbContext _context;

        [GlobalSetup]
        public void Setup()
        {
            _context = new BenchmarkDbContext();

            var products = Enumerable.Range(1, 10000)
                .Select(i => new Product { Id = i, Name = $"Product {i}" })
                .ToList();

            _context.Products.AddRange(products);
            _context.SaveChanges();
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task PaginateAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var result = await query.PaginateAsync(1, pageSize);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task SimplePaginateAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var result = await query.SimplePaginateAsync(1, pageSize);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PaginationBenchmarks>();
        }
    }
}