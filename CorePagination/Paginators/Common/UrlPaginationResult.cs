using CorePagination.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.Common
{
    /// <summary>
    /// Represents a pagination result that includes URL navigation properties.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
    public class UrlPaginationResult<T> : IPaginationResult<T>
    {
        /// <summary>
        /// Gets or sets the paginated items.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the URL for the next page.
        /// </summary>
        public string NextUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL for the previous page.
        /// </summary>
        public string PreviousUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL for the current page.
        /// </summary>
        public string CurrentUrl { get; internal set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the total number of items. This value may be null if the total count is not available.
        /// </summary>
        public int? TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the URL for the first page.
        /// </summary>
        public string FirstPageUrl { get; internal set; }

        /// <summary>
        /// Gets or sets the URL for the last page. This value may be null if the total count is not available.
        /// </summary>
        public string? LastPageUrl { get; internal set; }

        /// <summary>
        /// Gets or sets the URL for the next page. This value may be null if there is no next page.
        /// </summary>
        public string? NextPageUrl { get; internal set; }

        /// <summary>
        /// Gets or sets the URL for the previous page. This value may be null if there is no previous page.
        /// </summary>
        public string? PreviousPageUrl { get; internal set; }
    }

}
