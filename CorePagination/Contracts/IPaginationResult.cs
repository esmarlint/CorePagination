using CorePagination.Paginators.Common;

namespace CorePagination.Contracts
{
    /// <summary>
    /// Represents the result of a pagination operation.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
    public interface IPaginationResult<T>
    {
        /// <summary>
        /// Gets or sets the paginated items.
        /// </summary>
        IEnumerable<T> Items { get; set; }
        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        int Page { get; set; }
        /// <summary>
        /// Gets or sets the total number of items. This value may be null if the total count is not available.
        /// </summary>
        int? TotalItems { get; set; }
    }
}
