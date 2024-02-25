using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Support;
using CorePagination.Tranformation.Contracts;

namespace CorePagination.Tranformation.Transformers
{
    public class SimpleUrlResultTransformer<T> : IPaginationTranformer<T, UrlPaginationResult<T>> where T : class
    {
        private readonly string _baseUrl;

        public SimpleUrlResultTransformer(string baseUrl)
        {
            Guard.NotNull(baseUrl, nameof(baseUrl));
            _baseUrl = baseUrl;
        }

        public UrlPaginationResult<T> Transform(IPaginationResult<T> paginationResult)
        {
            Guard.NotNull(paginationResult, nameof(paginationResult));

            var currentPage = paginationResult.Page;
            var hasNextPage = true;
            var hasPrevPage = currentPage > 1;

            return new UrlPaginationResult<T>
            {
                Items = paginationResult.Items,
                PageSize = paginationResult.PageSize,
                FirstPageUrl = $"{_baseUrl}?page=1",
                NextUrl = hasNextPage ? $"{_baseUrl}?page={currentPage + 1}" : null,
                PreviusUrl = hasPrevPage ? $"{_baseUrl}?page={currentPage - 1}" : null,
                CurrentUrl = $"{_baseUrl}?page={currentPage}"
            };
        }
    }

}
