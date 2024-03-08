using CorePagination.Paginators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Contracts
{
    /// <summary>
    /// Defines the contract for pagination operations.
    /// </summary>
    /// <typeparam name="T">The type of the elements to be paginated.</typeparam>
    /// <typeparam name="TParameters">The type of the pagination parameters.</typeparam>
    /// <typeparam name="TResult">The type of the pagination result.</typeparam>
    public interface IPagination<T, TParameters, TResult>
        where TParameters : PagePaginatorParameters
        where TResult : class
    {
        /// <summary>
        /// Paginates the provided IQueryable based on the given pagination parameters.
        /// </summary>
        /// <param name="query">The IQueryable to be paginated.</param>
        /// <param name="parameters">The parameters for pagination.</param>
        /// <returns>A task that represents the asynchronous pagination operation. The task result contains the pagination result.</returns>
        Task<TResult> PaginateAsync(IQueryable<T> query, TParameters parameters);
    }
}
