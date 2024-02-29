using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Support;
using CorePagination.Tranformation.Contracts;

namespace CorePagination.Tranformation.Transformers
{
    /// <summary>
    /// Transforms a pagination result into a URL-enhanced pagination result, adding navigational links for a simpler user interface interaction.
    /// </summary>
    /// <remarks>
    /// This transformer is ideal for web APIs where the consumer needs to navigate through pages of results. The transformer appends URL links for next, previous, first, and potentially last pages based on the pagination data.
    /// </remarks>
    /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
    public class SimpleUrlResultTransformer<T> : IPaginationTranformer<T, UrlPaginationResult<T>> where T : class
    {
        private readonly string _baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleUrlResultTransformer{T}"/> class using the specified base URL for link generation.
        /// </summary>
        /// <param name="baseUrl">The base URL to be used for generating navigational links. This URL should not include pagination query parameters.</param>
        /// <exception cref="ArgumentNullException">Thrown if the baseUrl is null or empty.</exception>
        public SimpleUrlResultTransformer(string baseUrl)
        {
            Guard.NotNull(baseUrl, nameof(baseUrl));
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Transforms the given pagination result into a <see cref="UrlPaginationResult{T}"/>, 
        /// adding URL navigation links based on the specified base URL.
        /// </summary>
        /// <param name="paginationResult">The pagination result to be transformed.</param>
        /// <returns>A <see cref="UrlPaginationResult{T}"/> that includes navigational URLs for the paginated data.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the paginationResult is null.</exception>
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
