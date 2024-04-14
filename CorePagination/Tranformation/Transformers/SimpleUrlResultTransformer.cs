using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Support;
using CorePagination.Tranformation.Contracts;

namespace CorePagination.Tranformation.Transformers
{
    /// <summary>
    /// Transforms a pagination result into a URL-enhanced pagination result, adding navigational links for easier user interface interaction.
    /// </summary>
    /// <remarks>
    /// Ideal for web APIs where consumers need to navigate through paginated results. It appends URL links for navigating between pages.
    /// </remarks>
    /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
    public class SimpleUrlResultTransformer<T> : UrlResultTransformerBase<T, UrlPaginationResult<T>> where T : class
    {
        private readonly string _baseUrl;
        private readonly Dictionary<string, string> _parameterRenames = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleUrlResultTransformer{T}"/> class using the specified base URL for link generation.
        /// </summary>
        /// <param name="baseUrl">The base URL used for generating navigational links. This URL should not include any pagination query parameters.</param>
        /// <exception cref="ArgumentNullException">Thrown if the baseUrl is null or empty.</exception>
        public SimpleUrlResultTransformer(string baseUrl = "")
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Transforms the given pagination result into a <see cref="UrlPaginationResult{T}"/>, 
        /// adding URL navigation links based on the specified base URL.
        /// </summary>
        /// <param name="paginationResult">The pagination result to be transformed.</param>
        /// <returns>A <see cref="UrlPaginationResult{T}"/> that includes navigational URLs for the paginated data.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the paginationResult is null.</exception>
        public override UrlPaginationResult<T> Transform(IPaginationResult<T> paginationResult)
        {
            Guard.NotNull(paginationResult, nameof(paginationResult));

            var currentPage = paginationResult.Page;
            var pageSize = paginationResult.PageSize;
            var hasNextPage = paginationResult.Items.Count() == pageSize;
            var hasPrevPage = currentPage > 1;

            var baseQueryString = _parametersToInclude.Select(kv =>
            {
                string value = kv.Key switch
                {
                    "page" => currentPage.ToString(),
                    "pageSize" => pageSize.ToString(),
                    _ => kv.Value
                };
                return $"{_parameterRenames.GetValueOrDefault(kv.Key, kv.Key)}={value}";
            }).ToList();

            string BuildUrl(int page) => $"{_baseUrl}?{string.Join("&", baseQueryString)}".Replace($"page={currentPage}", $"page={page}");

            return new UrlPaginationResult<T>
            {
                Items = paginationResult.Items,
                PageSize = pageSize,
                Page = currentPage,
                TotalItems = paginationResult.TotalItems,
                FirstPageUrl = BuildUrl(1),
                PreviousUrl = hasPrevPage ? BuildUrl(currentPage - 1) : null,
                CurrentUrl = BuildUrl(currentPage),
                NextUrl = hasNextPage ? BuildUrl(currentPage + 1) : null
            };
        }

    }

}
