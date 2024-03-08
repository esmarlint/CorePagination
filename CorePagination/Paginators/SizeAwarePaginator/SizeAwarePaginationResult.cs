using CorePagination.Paginators.Common;

namespace CorePagination.Paginators.SizeAwarePaginator
{
    /// <summary>
    /// Represents the result of a pagination operation that includes total item and page counts.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
    public class SizeAwarePaginationResult<T> : PaginationResult<T>
    {
        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }
    }
}