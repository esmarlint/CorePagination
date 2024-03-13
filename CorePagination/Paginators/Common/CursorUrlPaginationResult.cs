using CorePagination.Paginators.CursorPaginator;

namespace CorePagination.Paginators.Common
{
    /// <summary>
    /// Represents a cursor-based pagination result that includes URL navigation properties.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
    /// <typeparam name="TKey">The type of the cursor.</typeparam>
    public class CursorUrlPaginationResult<T, TKey> : CursorPaginationResult<T, TKey>
        where T : class
        where TKey : IComparable
    {
        /// <summary>
        /// Gets or sets the URL for the next page.
        /// </summary>
        public string NextPageUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL for the current page.
        /// </summary>
        public string CurrentUrl { get; set; }
    }

}
