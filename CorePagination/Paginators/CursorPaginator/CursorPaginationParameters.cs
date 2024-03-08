using CorePagination.Common;
using CorePagination.Paginators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.CursorPaginator
{
    /// <summary>
    /// Represents the parameters for cursor-based pagination.
    /// </summary>
    /// <typeparam name="TKey">The type of the cursor property.</typeparam>
    public class CursorPaginationParameters<TKey> : PagePaginatorParameters
    {
        /// <summary>
        /// Gets or sets the current cursor value.
        /// </summary>
        public TKey CurrentCursor { get; set; }
        /// <summary>
        /// Gets or sets the pagination order (ascending or descending). Default is ascending.
        /// </summary>
        public PaginationOrder Order { get; set; } = PaginationOrder.Ascending;
    }
}
