using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using Microsoft.EntityFrameworkCore;

namespace CorePagination.Paginators.SizeAwarePaginator
{
    public class SizeAwarePaginator<T> : IPagination<T, PaginatorParameters, SizeAwarePaginationResult<T>>
    {
        public async Task<SizeAwarePaginationResult<T>> PaginateAsync(IQueryable<T> query, PaginatorParameters parameters)
        {
            var items = await query.Skip((parameters.Page - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync();
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize);

            return new SizeAwarePaginationResult<T>
            {
                Items = items,
                PageSize = parameters.PageSize,
                Page = parameters.Page,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public async Task<SizeAwarePaginationResult<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };
            return await PaginateAsync(query, parameters);
        }
    }
}