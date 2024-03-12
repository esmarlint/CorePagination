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

        public static UrlPaginationResult<T> WithUrl<T>(this IPaginationResult<T> paginationResult, string baseUrl)
            where T : class
        {
            var transformer = new SizeAwareUrlResultTransformer<T>(baseUrl);
            return transformer.Transform(paginationResult);
        }

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
