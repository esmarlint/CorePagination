using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Tranformation.Transformers;

namespace CorePagination.Tranformation.Extensions
{
    public static class PaginationExtensions
    {
        public static UrlPaginationResult<T> WithSimpleUrl<T>(this IPaginationResult<T> paginationResult, string baseUrl)
                 where T : class
        {
            var transformer = new SimpleUrlResultTransformer<T>(baseUrl);
            return transformer.Transform(paginationResult);
        }

        /// <summary>
        /// Applies a URL transformation to the pagination result using the SizeAwareUrlResultTransformer.
        /// </summary>
        /// <param name="paginationResult">The pagination result to transform.</param>
        /// <param name="baseUrl">The base URL to be used for generating navigational links.</param>
        /// <returns>A UrlPaginationResult with the transformed URLs included.</returns>
        /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
        public static UrlPaginationResult<T> WithUrl<T>(this IPaginationResult<T> paginationResult, string baseUrl)
            where T : class
        {
            var transformer = new SizeAwareUrlResultTransformer<T>(baseUrl);
            return transformer.Transform(paginationResult);
        }

        /// <summary>
        /// Applies a URL transformation to the pagination result using the CursorUrlResultTransformer.
        /// </summary>
        /// <param name="paginationResult">The pagination result to transform.</param>
        /// <param name="baseUrl">The base URL to be used for generating navigational links.</param>
        /// <returns>A CursorUrlPaginationResult with the transformed URLs included.</returns>
        /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
        /// <typeparam name="TKey">The type of the key used for cursor pagination, which must be comparable.</typeparam>
        public static CursorUrlPaginationResult<T, TKey> WithUrl<T, TKey>(
            this IPaginationResult<T> paginationResult, string baseUrl)
            where T : class
            where TKey : IComparable
        {
            var transformer = new CursorUrlResultTranformer<T, TKey>(baseUrl);
            return (CursorUrlPaginationResult<T, TKey>)transformer.Transform(paginationResult);
        }
    }
}
