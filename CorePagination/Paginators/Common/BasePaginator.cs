using CorePagination.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.Common
{
    public abstract class AbstractPaginator<T, TParameters, TResult>
        where TParameters : PagePaginatorParameters
        where TResult : class, IPaginationResult<T>, new()
    {
        public abstract Task<TResult> PaginateAsync(IQueryable<T> query, TParameters parameters);
        public abstract Task<TResult> NextPage(IQueryable<T> query, TParameters parameters);
        public abstract Task<TResult> PreviousPage(IQueryable<T> query, TParameters parameters);
    }

}
