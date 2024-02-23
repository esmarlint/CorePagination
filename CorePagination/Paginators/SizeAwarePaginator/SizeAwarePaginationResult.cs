using CorePagination.Paginators.Common;

namespace CorePagination.Paginators.SizeAwarePaginator
{
    public class SizeAwarePaginationResult<T> : PaginationResult<T>
    {
        public int TotalPages { get; set; }
    }
}