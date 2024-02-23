using CorePagination.Contracts;

namespace CorePagination.Tranformation.Extensions
{
    public static class PaginationTransformationExtensions
    {
        public static TResult Transform<T, TResult>(
            this IPaginationResult<T> paginationResult,
            Func<IPaginationResult<T>, TResult> transformation) where TResult : class
        {
            return transformation(paginationResult);
        }

        public static TResult TransformWithItems<T, TResult>(
            this IPaginationResult<T> paginationResult,
            Func<IPaginationResult<T>, TResult> transformation) where TResult : IPaginationResult<T>, new()
        {
            var result = transformation(paginationResult);
            if (result is IPaginationResult<T> paginationResultWithItems)
            {
                paginationResultWithItems.Items = paginationResult.Items;
            }
            return result;
        }
    }
}
