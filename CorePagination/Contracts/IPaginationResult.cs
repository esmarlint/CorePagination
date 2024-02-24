using CorePagination.Paginators.Common;

namespace CorePagination.Contracts
{
    public interface IPaginationResult<T>
    {
        IEnumerable<T> Items { get; set; }
        int PageSize { get; set; }
        int Page { get; set; }
        int? TotalItems { get; set; }
    }
}
