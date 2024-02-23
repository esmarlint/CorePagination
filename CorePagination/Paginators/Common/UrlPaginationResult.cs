using CorePagination.Contracts;
using CorePagination.Paginators.CursorPaginator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.Common
{
    public class UrlPaginationResult<T> : IPaginationResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageSize { get; set; }
        public string NextUrl { get; set; }
        public string PreviusUrl { get; set; }
        public string CurrentUrl { get; internal set; }
        public int Page { get; set; }
        public int? TotalItems { get; set; }
        public string FirstPageUrl { get; internal set; }
        public string? LastPageUrl { get; internal set; }
        public string? NextPageUrl { get; internal set; }
        public string? PreviousPageUrl { get; internal set; }
    }

    public class CursorUrlPaginationResult<T, TKey> : CursorPaginationResult<T, TKey>
        where T : class
        where TKey : IComparable
    {
        public string NextPageUrl { get; set; }
        public string CurrentUrl { get; set; }
    }

}
