using CorePagination.Common;
using CorePagination.Paginators.Common;
using CorePagination.Paginators.CursorPaginator;
using CorePagination.Paginators.SimplePaginator;
using CorePagination.Paginators.SizeAwarePaginator;
using CorePagination.Support;
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
        /// <summary>
        /// Paginates a queryable source based on the specified page number and page size.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="query">The queryable source to paginate.</param>
        /// <param name="pageNumber">The one-based page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A <see cref="PaginationResult{T}"/> containing the paginated result.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the query is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if pageNumber or pageSize is less than one.</exception>
        public static PaginationResult<T> SimplePaginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            Guard.NotNull(query, nameof(query));
            Guard.GreaterThanZero(pageNumber, nameof(pageNumber));


            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginationResult<T>
            {
                Items = items,
                PageSize = pageSize,
                Page = pageNumber,
                TotalItems = null
            };
        }

        /// <summary>
        /// Paginates a queryable source based on the provided pagination parameters, including total item and page counts.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="query">The queryable source to paginate.</param>
        /// <param name="pageNumber">The one-based page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A <see cref="SizeAwarePaginationResult{T}"/> containing the paginated result with total counts.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the query is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if pageNumber or pageSize is less than one.</exception>
        public static SizeAwarePaginationResult<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            Guard.NotNull(query, nameof(query));
            Guard.GreaterThanZero(pageNumber, nameof(pageNumber));


            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new SizeAwarePaginationResult<T>
            {
                Items = items,
                PageSize = pageSize,
                Page = pageNumber,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        /// <summary>
        /// Paginates a queryable source based on cursor parameters, suitable for efficient, stateless pagination.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <typeparam name="TKey">The type used for the cursor property.</typeparam>
        /// <param name="query">The queryable source to paginate.</param>
        /// <param name="keySelector">The expression used to select the cursor property.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="currentCursor">The current cursor value. Default is the default value of <typeparamref name="TKey"/>.</param>
        /// <param name="order">The pagination order. Default is <see cref="PaginationOrder.Ascending"/>.</param>
        /// <returns>A <see cref="CursorPaginationResult{T, TKey}"/> containing the paginated result with cursor information.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the query or keySelector is null.</exception>
        public static CursorPaginationResult<T, TKey> CursorPaginate<T, TKey>(
            this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, int pageSize, TKey currentCursor = default, PaginationOrder order = PaginationOrder.Ascending)
            where T : class
            where TKey : IComparable
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNull(keySelector, nameof(keySelector));

            var paginator = new CursorPaginator<T, TKey>(keySelector);
            var parameters = new CursorPaginationParameters<TKey> { PageSize = pageSize, CurrentCursor = currentCursor, Order = order };
            return paginator.Paginate(query, parameters);
        }

        /// <summary>
        /// Asynchronously paginates a queryable source based on the specified page number and page size.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="query">The queryable source to paginate.</param>
        /// <param name="pageNumber">The one-based page index.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="PaginationResult{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the query is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if pageNumber or pageSize are less than one.</exception>
        public static async Task<PaginationResult<T>> SimplePaginateAsync<T>(
            this IQueryable<T> query, int pageNumber, int pageSize)
        {
            Guard.NotNull(query, nameof(query));
            Guard.GreaterThanZero(pageNumber, nameof(pageNumber));


            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginationResult<T>
            {
                Items = items,
                PageSize = pageSize,
                Page = pageNumber,
                TotalItems = null
            };
        }

        /// <summary>
        /// Asynchronously paginates a queryable source based on the provided pagination parameters, including total item and page counts.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="query">The queryable source to paginate.</param>
        /// <param name="pageNumber">The one-based page index.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="SizeAwarePaginationResult{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the query is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if pageNumber or pageSize are less than one.</exception>
        public static async Task<SizeAwarePaginationResult<T>> PaginateAsync<T>(
            this IQueryable<T> query, int pageNumber, int pageSize)
        {
            Guard.NotNull(query, nameof(query));
            Guard.GreaterThanZero(pageNumber, nameof(pageNumber));


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

        /// <summary>
        /// Asynchronously paginates a queryable source based on cursor parameters, suitable for implementing efficient, stateful pagination.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <typeparam name="TKey">The type used for the cursor property.</typeparam>
        /// <param name="query">The queryable source to paginate.</param>
        /// <param name="parameters">The cursor pagination parameters including the page size, current cursor, and pagination order.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="CursorPaginationResult{T, TKey}"/>, 
        /// which includes the set of items for the current page, the current cursor, and an indication if more items are available.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the query is null or if the parameters are null.</exception>
        public static async Task<CursorPaginationResult<T, TKey>> CursorPaginateAsync<T, TKey>(
            this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, int pageSize, TKey currentCursor = default, PaginationOrder order = PaginationOrder.Ascending)
            where T : class
            where TKey : IComparable
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNull(keySelector, nameof(keySelector));

            var paginator = new CursorPaginator<T, TKey>(keySelector);
            var parameters = new CursorPaginationParameters<TKey> { PageSize = pageSize, CurrentCursor = currentCursor, Order = order };
            return await paginator.PaginateAsync(query, parameters);
        }
    }
}
