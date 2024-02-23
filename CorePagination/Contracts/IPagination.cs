using CorePagination.Paginators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Contracts
{
    public interface IPagination<T, TParameters, TResult>
        where TParameters : PagePaginatorParameters
        where TResult : class
    {
        Task<TResult> PaginateAsync(IQueryable<T> query, TParameters parameters);
    }
}
