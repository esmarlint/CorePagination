using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CorePagination.Extensions;
using System.Linq;

namespace CorePagination.Benchmarks
{
    [MemoryDiagnoser]
    public class PaginationBenchmarks
    {
        private readonly IQueryable<Product> _data;

        public PaginationBenchmarks()
        {
            _data = Enumerable.Range(1, 10000).Select(i => new Product { Id = i, Name = $"Product {i}" }).AsQueryable();
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task PaginateAsync(int totalItems, int pageSize)
        {
            var result = await _data.Take(totalItems).PaginateAsync(1, pageSize);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task SimplePaginateAsync(int totalItems, int pageSize)
        {
            var result = await _data.Take(totalItems).SimplePaginateAsync(1, pageSize);
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