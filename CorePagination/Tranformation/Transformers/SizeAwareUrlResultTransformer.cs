using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Support;
using CorePagination.Tranformation.Contracts;

namespace CorePagination.Tranformation.Transformers
{
    public class SizeAwareUrlResultTransformer<T> : UrlResultTransformerBase<T, UrlPaginationResult<T>> where T : class
    {
        private readonly string _baseUrl;
        private bool _includeTotalItems;
        private bool _includeTotalPages;

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
        public override UrlPaginationResult<T> Transform(IPaginationResult<T> paginationResult)
        {
            Guard.NotNull(paginationResult, nameof(paginationResult));

            var currentPage = paginationResult.Page;
            var totalItems = paginationResult.TotalItems ?? 0;
            var pageSize = paginationResult.PageSize;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var queryParams = new List<string>
            {
                $"page={currentPage}",
                $"pageSize={pageSize}"
            };

            if (_includeTotalItems)
            {
                queryParams.Add($"totalItems={totalItems}");
            }

            if (_includeTotalPages)
            {
                queryParams.Add($"totalPages={totalPages}");
            }

            var queryString = string.Join("&", queryParams);
            var baseUrlWithParams = $"{_baseUrl}?{queryString}";

            return new UrlPaginationResult<T>
            {
                Items = paginationResult.Items,
                PageSize = pageSize,
                Page = currentPage,
                TotalItems = totalItems,
                FirstPageUrl = $"{baseUrlWithParams}&page=1",
                LastPageUrl = $"{baseUrlWithParams}&page={totalPages}",
                NextPageUrl = currentPage < totalPages ? $"{baseUrlWithParams}&page={currentPage + 1}" : null,
                PreviousPageUrl = currentPage > 1 ? $"{baseUrlWithParams}&page={currentPage - 1}" : null,
                CurrentUrl = baseUrlWithParams,
            };
        }

        #region Fluent API
        public SizeAwareUrlResultTransformer<T> IncludeTotalItems()
        {
            _includeTotalItems = true;
            return this;
        }

        public SizeAwareUrlResultTransformer<T> IncludeTotalPages()
        {
            _includeTotalPages = true;
            return this;
        }
        #endregion

    }

}
