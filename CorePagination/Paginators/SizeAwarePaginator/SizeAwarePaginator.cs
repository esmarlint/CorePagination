using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Support;
using Microsoft.EntityFrameworkCore;

namespace CorePagination.Paginators.SizeAwarePaginator
{

    /// <summary>
    /// Represents a paginator that provides pagination functionality with total item and page count.
    /// </summary>
    /// <typeparam name="T">The type of the elements to be paginated.</typeparam>
    public class SizeAwarePaginator<T> : IPaginationAsync<T, PaginatorParameters, SizeAwarePaginationResult<T>>
    {
        /// <summary>
        /// Paginates the provided IQueryable based on the given PaginatorParameters and calculates total item and page count.
        /// </summary>
        /// <param name="query">The IQueryable to be paginated.</param>
        /// <param name="parameters">The parameters for pagination, including page number and page size.</param>
        /// <returns>A SizeAwarePaginationResult containing the paginated items, total item count, total page count, and other pagination information.</returns>
        public SizeAwarePaginationResult<T> Paginate(IQueryable<T> query, PaginatorParameters parameters)
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNull(parameters, nameof(parameters));
            Guard.GreaterThanZero(parameters.Page, nameof(parameters.Page));

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)parameters.PageSize);
            var items = query.Skip((parameters.Page - 1) * parameters.PageSize).Take(parameters.PageSize).ToList();

            return new SizeAwarePaginationResult<T>
            {
                Items = items,
                PageSize = parameters.PageSize,
                Page = parameters.Page,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }

        /// <summary>
        /// Paginates the provided IQueryable based on the given PaginatorParameters and calculates total item and page count.
        /// </summary>
        /// <param name="query">The IQueryable to be paginated.</param>
        /// <param name="parameters">The parameters for pagination, including page number and page size.</param>
        /// <returns>A SizeAwarePaginationResult containing the paginated items, total item count, total page count, and other pagination information.</returns>
        public async Task<SizeAwarePaginationResult<T>> PaginateAsync(IQueryable<T> query, PaginatorParameters parameters)
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNull(parameters, nameof(parameters));
            Guard.GreaterThanZero(parameters.Page, nameof(parameters.Page));


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

        /// <summary>
        /// Paginates the provided IQueryable based on the given page number and page size, and calculates total item and page count.
        /// </summary>
        /// <param name="query">The IQueryable to be paginated.</param>
        /// <param name="pageNumber">The page number of the desired page.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A SizeAwarePaginationResult containing the paginated items, total item count, total page count, and other pagination information.</returns>
        public async Task<SizeAwarePaginationResult<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };
            return await PaginateAsync(query, parameters);
        }
    }
}