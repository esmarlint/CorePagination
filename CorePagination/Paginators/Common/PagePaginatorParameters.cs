using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.Common
{
    /// <summary>
    /// Represents the base parameters for page-based pagination.
    /// </summary>
    public class PagePaginatorParameters
    {
        /// <summary>
        /// Gets or sets the number of items per page. Default is 10.
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
