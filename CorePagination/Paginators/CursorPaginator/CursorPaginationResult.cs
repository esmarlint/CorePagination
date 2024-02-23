using CorePagination.Paginators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.CursorPaginator
{
    public class CursorPaginationResult<T, TKey> : PaginationResult<T>
    {
        public TKey CurrentCursor { get; set; }
        public TKey NextCursor { get; set; }
        public bool HasMore { get; set; }
    }
}
