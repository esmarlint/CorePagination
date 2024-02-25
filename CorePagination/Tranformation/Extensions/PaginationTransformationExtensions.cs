using CorePagination.Contracts;
using CorePagination.Paginators.SimplePaginator;
using CorePagination.Support;

namespace CorePagination.Tranformation.Extensions
{
    public static class PaginationTransformationExtensions
    {
        public static TResult Transform<T, TResult>(
            this IPaginationResult<T> paginationResult,
            Func<IPaginationResult<T>, TResult> transformation) where TResult : class
        {
            Guard.NotNull(paginationResult, nameof(paginationResult));

            return transformation(paginationResult);
        }

        public static TResult TransformWithItems<T, TResult>(
            this IPaginationResult<T> paginationResult,
            Func<IPaginationResult<T>, TResult> transformation) where TResult : IPaginationResult<T>, new()
        {
            Guard.NotNull(paginationResult, nameof(paginationResult));

            var result = transformation(paginationResult);
            if (result is IPaginationResult<T> paginationResultWithItems)
            {
                paginationResultWithItems.Items = paginationResult.Items;
            }
            return result;
        }
    }
}
