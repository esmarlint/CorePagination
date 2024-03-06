using CorePagination.Contracts;
using CorePagination.Paginators.SimplePaginator;
using CorePagination.Support;

namespace CorePagination.Tranformation.Extensions
{
    public static class PaginationTransformationExtensions
    {
        /// <summary>
        /// Transforms the specified pagination result using the provided transformation function.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the original pagination result.</typeparam>
        /// <typeparam name="TResult">The type of the transformed pagination result.</typeparam>
        /// <param name="paginationResult">The original pagination result to transform.</param>
        /// <param name="transformation">A function that transforms the pagination result into the desired format.</param>
        /// <returns>The transformed pagination result.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the paginationResult or the transformation function is null.</exception>
        public static TResult Transform<T, TResult>(
            this IPaginationResult<T> paginationResult,
            Func<IPaginationResult<T>, TResult> transformation) where TResult : class
        {
            Guard.NotNull(paginationResult, nameof(paginationResult));

            return transformation(paginationResult);
        }

        /// <summary>
        /// Transforms the specified pagination result using the provided transformation function, including transformation of the items within the result.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the original pagination result.</typeparam>
        /// <typeparam name="TResult">The type of the transformed pagination result where TResult must implement IPaginationResult&lt;T&gt;.</typeparam>
        /// <param name="paginationResult">The original pagination result to transform.</param>
        /// <param name="transformation">A function that transforms both the pagination result and its items.</param>
        /// <returns>The transformed pagination result with transformed items.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the paginationResult or the transformation function is null.</exception>
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
