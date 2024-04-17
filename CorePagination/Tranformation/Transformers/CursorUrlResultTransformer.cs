using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Paginators.CursorPaginator;
using CorePagination.Support;
using CorePagination.Tranformation.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Tranformation.Transformers
{
    /// <summary>
    /// Transforms a cursor-based pagination result into a <see cref="CursorUrlPaginationResult{T, TKey}"/>, 
    /// enhancing it with navigational URLs for cursor-based pagination.
    /// </summary>
    /// <remarks>
    /// This transformer is particularly useful in APIs where cursor-based navigation is implemented, 
    /// allowing clients to navigate through the dataset using cursors instead of page numbers. The transformed
    /// result includes URLs that clients can use to request the next set of results or go back to the previous set.
    /// </remarks>
    /// <typeparam name="T">The type of the elements in the pagination result.</typeparam>
    /// <typeparam name="TKey">The type of the cursor.</typeparam>
    public class CursorUrlResultTransformer<T, TKey> : UrlResultTransformerBase<T, CursorUrlPaginationResult<T, TKey>>
        where T : class
        where TKey : IComparable
    {
        private readonly string _baseUrl;
        private bool _includeCurrentCursor;
        private bool _includeNextCursor;
        private bool _includeDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CursorUrlResultTransformer{T, TKey}"/> class using the specified base URL for link generation.
        /// </summary>
        /// <param name="baseUrl">The base URL to be used for generating navigational links. This URL should not include cursor query parameters.</param>
        /// <exception cref="ArgumentNullException">Thrown if the baseUrl is null or empty.</exception>
        public CursorUrlResultTransformer(string baseUrl)
        {
            Guard.NotNull(baseUrl, nameof(baseUrl));
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Transforms the given pagination result into a <see cref="CursorUrlPaginationResult{T, TKey}"/>
        /// by appending navigation links appropriate for cursor-based pagination.
        /// </summary>
        /// <param name="paginationResult">The pagination result to transform.</param>
        /// <returns>A <see cref="CursorUrlPaginationResult{T, TKey}"/> that includes cursor-based navigational URLs.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the pagination result is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the pagination result is not compatible with cursor-based pagination.</exception>
        public override CursorUrlPaginationResult<T, TKey> Transform(IPaginationResult<T> paginationResult)
        {
            Guard.NotNull(paginationResult, nameof(paginationResult));

            if (!(paginationResult is CursorPaginationResult<T, TKey> cursorPaginationResult))
            {
                throw new ArgumentException("The pagination result must be of type CursorPaginationResult<T, TKey>.", nameof(paginationResult));
            }

            var queryParams = new List<string>
            {
                $"pageSize={cursorPaginationResult.PageSize}",
                $"currentCursor={cursorPaginationResult.CurrentCursor}"
            };

            var queryString = string.Join("&", queryParams);
            var baseUrlWithParams = $"{_baseUrl}?{queryString}";

            return new CursorUrlPaginationResult<T, TKey>
            {
                Items = cursorPaginationResult.Items,
                PageSize = cursorPaginationResult.PageSize,
                CurrentCursor = cursorPaginationResult.CurrentCursor,
                NextCursor = cursorPaginationResult.NextCursor,
                CurrentUrl = baseUrlWithParams,
                NextPageUrl = cursorPaginationResult.HasMore ? baseUrlWithParams.Replace($"currentCursor={cursorPaginationResult.CurrentCursor}", $"currentCursor={cursorPaginationResult.NextCursor}") : null
            };
        }

        #region Fluent API
        /// <summary>
        /// Configures the transformer to include the current cursor value in the pagination results.
        /// </summary>
        /// <returns>The instance of <see cref="CursorUrlResultTransformer{T, TKey}"/> for further configuration.</returns>
        public CursorUrlResultTransformer<T, TKey> IncludeCurrentCursor()
        {
            _includeCurrentCursor = true;
            return this;
        }

        /// <summary>
        /// Configures the transformer to include the next cursor value, facilitating forward navigation in the pagination results.
        /// </summary>
        /// <returns>The instance of <see cref="CursorUrlResultTransformer{T, TKey}"/> for further configuration.</returns>
        public CursorUrlResultTransformer<T, TKey> IncludeNextCursor()
        {
            _includeNextCursor = true;
            return this;
        }

        #endregion
    }

}
