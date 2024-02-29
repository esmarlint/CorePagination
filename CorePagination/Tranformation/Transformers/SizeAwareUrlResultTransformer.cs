using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Support;
using CorePagination.Tranformation.Contracts;

namespace CorePagination.Tranformation.Transformers
{
    public class SizeAwareUrlResultTransformer<T> : IPaginationTranformer<T, UrlPaginationResult<T>> where T : class
    {
        private readonly string _baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeAwareUrlResultTransformer{T}"/> class.
        /// This constructor sets up the transformer with the base URL to be used for generating navigation links.
        /// </summary>
        /// <param name="baseUrl">The base URL to be used for appending navigation links to the pagination results.</param>
        /// <exception cref="ArgumentNullException">Thrown when the baseUrl is null or empty.</exception>
        public SizeAwareUrlResultTransformer(string baseUrl)
        {
            Guard.NotNull(baseUrl, nameof(baseUrl));
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Transforms a given pagination result into a URL-enhanced pagination result.
        /// This transformer is particularly useful for adding navigation links to API responses.
        /// </summary>
        /// <param name="paginationResult">The pagination result to transform.</param>
        /// <returns>A <see cref="UrlPaginationResult{T}"/> that includes navigational URLs based on the current pagination state.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the input pagination result is null.</exception>
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
