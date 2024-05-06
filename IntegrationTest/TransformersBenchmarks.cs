using BenchmarkDotNet.Attributes;
using CorePagination.Paginators.Common;
using CorePagination.Paginators.CursorPaginator;
using CorePagination.Paginators.SimplePaginator;
using CorePagination.Paginators.SizeAwarePaginator;
using CorePagination.Tranformation.Transformers;
using CorePagination.Tranformation.Extensions;

namespace CorePagination.Benchmarks
{
    [MemoryDiagnoser]
    public class TransformersBenchmarks
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
        public async Task SizeAwarePaginatorWithTransformerAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var parameters = new PaginatorParameters { Page = 1, PageSize = pageSize };
            var paginationResult = await _sizeAwarePaginator.PaginateAsync(query, parameters);
            var transformedResult = new SizeAwareUrlResultTransformer<Product>("/products").Transform(paginationResult);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task SizeAwarePaginatorWithTransformerExtensionAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var parameters = new PaginatorParameters { Page = 1, PageSize = pageSize };
            var paginationResult = await _sizeAwarePaginator.PaginateAsync(query, parameters);
            var transformedResult = paginationResult.WithUrl("/products");
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task CursorPaginatorWithTransformerAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems).OrderBy(p => p.Id);
            var parameters = new CursorPaginationParameters<int> { PageSize = pageSize };
            var paginationResult = await _cursorPaginator.PaginateAsync(query, parameters);
            var transformedResult = new CursorUrlResultTransformer<Product, int>("/products").Transform(paginationResult);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task CursorPaginatorWithTransformerExtensionAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems).OrderBy(p => p.Id);
            var parameters = new CursorPaginationParameters<int> { PageSize = pageSize };
            var paginationResult = await _cursorPaginator.PaginateAsync(query, parameters);
            var transformedResult = paginationResult.WithUrl<Product, int>("/products");
        }
    }
}