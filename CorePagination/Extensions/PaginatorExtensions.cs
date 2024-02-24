using CorePagination.Common;
using CorePagination.Paginators.Common;
using CorePagination.Paginators.CursorPaginator;
using CorePagination.Paginators.SimplePaginator;
using CorePagination.Paginators.SizeAwarePaginator;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Extensions
{
    public static class PaginatorExtensions
    {
        public static async Task<PaginationResult<T>> SimplePaginateAsync<T>(
            this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginationResult<T>
            {
                Items = items,
                PageSize = pageSize,
                Page = pageNumber,
                TotalItems = null
            };
        }

        public static async Task<SizeAwarePaginationResult<T>> PaginateAsync<T>(
            this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new SizeAwarePaginationResult<T>
            {
                Items = items,
                PageSize = pageSize,
                Page = pageNumber,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        public static async Task<CursorPaginationResult<T, TKey>> CursorPaginateAsync<T, TKey>(
            this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, int pageSize, TKey currentCursor = default, PaginationOrder order = PaginationOrder.Ascending)
            where T : class
            where TKey : IComparable
        {
            var paginator = new CursorPaginator<T, TKey>(keySelector);
            var parameters = new CursorPaginationParameters<TKey> { PageSize = pageSize, CurrentCursor = currentCursor, Order = order };
            return await paginator.PaginateAsync(query, parameters);
        }
    }
}
