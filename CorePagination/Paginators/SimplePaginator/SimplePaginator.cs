using CorePagination.Contracts;
using CorePagination.Paginators.Common;
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

    public abstract class AbstractPaginator<T, TParameters, TResult>
    where TParameters : PagePaginatorParameters
    where TResult : class, IPaginationResult<T>, new()
    {
        public abstract Task<TResult> PaginateAsync(IQueryable<T> query, TParameters parameters);

        public abstract Task<TResult> NextPage(IQueryable<T> query, TParameters parameters);

        public abstract Task<TResult> PreviousPage(IQueryable<T> query, TParameters parameters);
    }

}
