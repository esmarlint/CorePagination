using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.Common
{
    /// <summary>
    /// Represents the parameters for page-based pagination, including page number and page size.
    /// </summary>
    public class PaginatorParameters : PagePaginatorParameters
    {
        /// <summary>
        /// Gets or sets the current page number. Default is 1.
        /// </summary>
        public int Page { get; set; } = 1;
    }
}
