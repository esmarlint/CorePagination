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
    public class SimplePaginator<T> : IPagination<T, PaginatorParameters, PaginationResult<T>>
    {
        public async Task<PaginationResult<T>> PaginateAsync(IQueryable<T> query, PaginatorParameters parameters)
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNull(parameters, nameof(parameters));

            var items = await query.Skip((parameters.Page - 1) * parameters.PageSize).Take(parameters.PageSize).ToListAsync();
            return new PaginationResult<T>
            {
                Items = items,
                PageSize = parameters.PageSize,
                Page = parameters.Page,
                TotalItems = null
            };
        }

        public async Task<PaginationResult<T>> PaginateAsync(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };
            return await PaginateAsync(query, parameters);
        }
    }
}
