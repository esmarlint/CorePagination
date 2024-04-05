using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Support;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.SimplePaginator
{
    /// <summary>
    /// Represents a simple paginator that provides basic pagination functionality.
    /// </summary>
    /// <typeparam name="T">The type of the elements to be paginated.</typeparam>
    public class SimplePaginator<T> : IPaginationAsync<T, PaginatorParameters, PaginationResult<T>>
    {
        /// <summary>
        /// Paginates the provided IQueryable based on the given PaginatorParameters.
        /// </summary>
        /// <param name="query">The IQueryable to be paginated.</param>
        /// <param name="parameters">The parameters for pagination, including page number and page size.</param>
        /// <returns>A PaginationResult containing the paginated items and pagination information.</returns>
        public PaginationResult<T> Paginate(IQueryable<T> query, PaginatorParameters parameters)
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNull(parameters, nameof(parameters));
            Guard.GreaterThanZero(parameters.Page, nameof(parameters.Page));

            var items = query.Skip((parameters.Page - 1) * parameters.PageSize).Take(parameters.PageSize).ToList();
            return new PaginationResult<T>
            {
                Items = items,
                PageSize = parameters.PageSize,
                Page = parameters.Page,
                TotalItems = null
            };
        }

        /// <summary>
        /// Paginates the provided IQueryable based on the given PaginatorParameters.
        /// </summary>
        /// <param name="query">The IQueryable to be paginated.</param>
        /// <param name="parameters">The parameters for pagination, including page number and page size.</param>
        /// <returns>A PaginationResult containing the paginated items and pagination information.</returns>
        public async Task<PaginationResult<T>> PaginateAsync(IQueryable<T> query, PaginatorParameters parameters)
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNull(parameters, nameof(parameters));
            Guard.GreaterThanZero(parameters.Page, nameof(parameters.Page));

            var items = await query.Skip((parameters.Page - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync();
            return new PaginationResult<T>
            {
                Items = items,
                PageSize = parameters.PageSize,
                Page = parameters.Page,
                TotalItems = null
            };
        }

        /// <summary>
        /// Paginates the provided IQueryable based on the given page number and page size.
        /// </summary>
        /// <param name="query">The IQueryable to be paginated.</param>
        /// <param name="pageNumber">The page number of the desired page.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A PaginationResult containing the paginated items and pagination information.</returns>
        public async Task<PaginationResult<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };
            return await PaginateAsync(query, parameters);
        }
    }
}
