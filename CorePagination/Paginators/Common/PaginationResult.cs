using CorePagination.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.Common
{
    /// <summary>
    /// Represents the result of a basic pagination operation.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
    public class PaginationResult<T> : IPaginationResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int? TotalItems { get; set; }
    }
}
