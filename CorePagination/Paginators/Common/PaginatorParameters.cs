using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.Common
{
    public class PaginatorParameters : PagePaginatorParameters
    {
        public int Page { get; set; } = 1;
    }
}
