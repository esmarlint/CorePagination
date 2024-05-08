using BenchmarkDotNet.Attributes;
using CorePagination.Extensions;
using CorePagination.Paginators.Common;
using CorePagination.Paginators.CursorPaginator;
using CorePagination.Paginators.SimplePaginator;
using CorePagination.Paginators.SizeAwarePaginator;
using Microsoft.EntityFrameworkCore;

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

        #region SizeAwarePaginator Benchmarks

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task SizeAwarePaginatorAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var parameters = new PaginatorParameters { Page = 1, PageSize = pageSize };
            var result = await _sizeAwarePaginator.PaginateAsync(query, parameters);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public void SizeAwarePaginator(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var parameters = new PaginatorParameters { Page = 1, PageSize = pageSize };
            var result = _sizeAwarePaginator.Paginate(query, parameters);
        }

        #endregion

        #region SimplePaginator Benchmarks

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task SimplePaginatorAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var parameters = new PaginatorParameters { Page = 1, PageSize = pageSize };
            var result = await _simplePaginator.PaginateAsync(query, parameters);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public void SimplePaginator(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems);
            var parameters = new PaginatorParameters { Page = 1, PageSize = pageSize };
            var result = _simplePaginator.Paginate(query, parameters);
        }

        #endregion

        #region CursorPaginator Benchmarks

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public async Task CursorPaginatorAsync(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems).OrderBy(p => p.Id);
            var parameters = new CursorPaginationParameters<int> { PageSize = pageSize };
            var result = await _cursorPaginator.PaginateAsync(query, parameters);
        }

        [Benchmark]
        [Arguments(100, 10)]
        [Arguments(1000, 20)]
        public void CursorPaginator(int totalItems, int pageSize)
        {
            var query = _context.Products.Take(totalItems).OrderBy(p => p.Id);
            var parameters = new CursorPaginationParameters<int> { PageSize = pageSize };
            var result = _cursorPaginator.Paginate(query, parameters);
        }

        #endregion
    }
}