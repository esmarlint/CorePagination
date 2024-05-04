using BenchmarkDotNet.Attributes;
using CorePagination.Extensions;
using CorePagination.Paginators.CursorPaginator;
using CorePagination.Paginators.SimplePaginator;
using CorePagination.Paginators.SizeAwarePaginator;

namespace CorePagination.Benchmarks
{
    [MemoryDiagnoser]
    public class PaginatorBenchmarks
    {
        private BenchmarkDbContext _context;
        private SizeAwarePaginator<Product> _sizeAwarePaginator;
        private SimplePaginator<Product> _simplePaginator;
        private CursorPaginator<Product, int> _cursorPaginator;

        [GlobalSetup]
        public void Setup()
        {
            _context = new BenchmarkDbContext();

            var products = Enumerable.Range(1, 10000)
                .Select(i => new Product { Id = i, Name = $"Product {i}" })
                .ToList();

            _context.Products.AddRange(products);
            _context.SaveChanges();

            _sizeAwarePaginator = new SizeAwarePaginator<Product>();
            _simplePaginator = new SimplePaginator<Product>();
            _cursorPaginator = new CursorPaginator<Product, int>(p => p.Id);
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

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task CursorPaginateAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems).OrderBy(p => p.Id);
            var result = await query.CursorPaginateAsync(p => p.Id, pageSize);
        }

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
        public void SimplePaginate(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var result = query.SimplePaginate(1, pageSize);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public void CursorPaginate(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems).OrderBy(p => p.Id);
            var result = query.CursorPaginate(p => p.Id, pageSize);
        }
    }
}