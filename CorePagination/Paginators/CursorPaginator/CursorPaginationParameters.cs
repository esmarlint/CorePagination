using CorePagination.Common;
using CorePagination.Paginators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.CursorPaginator
{
    public class CursorPaginationParameters<TKey> : PagePaginatorParameters
    {
        public TKey CurrentCursor { get; set; }
        public PaginationOrder Order { get; set; } = PaginationOrder.Ascending;
    }
}
