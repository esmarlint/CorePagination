using BenchmarkDotNet.Attributes;
using CorePagination.Extensions;

namespace CorePagination.Benchmarks
{
    [MemoryDiagnoser]
    public class PaginatorExtensionsBenchmarks
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

        #region Asynchronous Pagination Benchmarks

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
        public async Task PaginateAndCountAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var result = await query.PaginateAndCountAsync(1, pageSize);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task CursorPaginateAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems).OrderBy(p => p.Id);
            var result = await query.PaginateCursorAsync(p => p.Id, pageSize);
        }

        #endregion

        #region Synchronous Pagination Benchmarks

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public void Paginate(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var result = query.Paginate(1, pageSize);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public void PaginateAndCount(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var result = query.PaginateAndCount(1, pageSize);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public void CursorPaginate(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems).OrderBy(p => p.Id);
            var result = query.PaginateCursor(p => p.Id, pageSize);
        }

        #endregion
    }
}