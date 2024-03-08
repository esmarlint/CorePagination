using CorePagination.Paginators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.CursorPaginator
{
    /// <summary>
    /// Represents the result of a cursor-based pagination operation.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
    /// <typeparam name="TKey">The type of the cursor property.</typeparam>
    public class CursorPaginationResult<T, TKey> : PaginationResult<T>
    {
        /// <summary>
        /// Gets or sets the current cursor value.
        /// </summary>
        public TKey CurrentCursor { get; set; }
        /// <summary>
        /// Gets or sets the next cursor value.
        /// </summary>
        public TKey NextCursor { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether there are more items available.
        /// </summary>
        public bool HasMore { get; set; }
    }
}
