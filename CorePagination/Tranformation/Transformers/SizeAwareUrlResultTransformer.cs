using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Support;
using CorePagination.Tranformation.Contracts;

namespace CorePagination.Tranformation.Transformers
{
    public class SizeAwareUrlResultTransformer<T> : IPaginationTranformer<T, UrlPaginationResult<T>> where T : class
    {
        private readonly string _baseUrl;

        public SizeAwareUrlResultTransformer(string baseUrl)
        {
            Guard.NotNull(baseUrl, nameof(baseUrl));
            _baseUrl = baseUrl;
        }

        public UrlPaginationResult<T> Transform(IPaginationResult<T> paginationResult)
        {
            Guard.NotNull(paginationResult, nameof(paginationResult));

            var currentPage = paginationResult.Page;
            var totalItems = paginationResult.TotalItems ?? 0;
            var pageSize = paginationResult.PageSize;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var hasNextPage = currentPage < totalPages;
            var hasPrevPage = currentPage > 1;

            return new UrlPaginationResult<T>
            {
                Items = paginationResult.Items,
                PageSize = pageSize,
                Page = currentPage,
                TotalItems = totalItems,
                FirstPageUrl = $"{_baseUrl}?page=1",
                LastPageUrl = $"{_baseUrl}?page={totalPages}",
                NextPageUrl = hasNextPage ? $"{_baseUrl}?page={currentPage + 1}" : null,
                PreviousPageUrl = hasPrevPage ? $"{_baseUrl}?page={currentPage - 1}" : null,
                CurrentUrl = $"{_baseUrl}?page={currentPage}",
            };
        }
    }

}
