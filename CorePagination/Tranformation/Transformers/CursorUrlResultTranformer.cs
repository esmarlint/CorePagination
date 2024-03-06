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
    public class CursorUrlResultTranformer<T, TKey> : IPaginationTranformer<T, CursorUrlPaginationResult<T, TKey>>
        where T : class
        where TKey : IComparable
    {
        private readonly string _baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="CursorUrlResultTransformer{T, TKey}"/> class using the specified base URL for link generation.
        /// </summary>
        /// <param name="baseUrl">The base URL to be used for generating navigational links. This URL should not include cursor query parameters.</param>
        /// <exception cref="ArgumentNullException">Thrown if the baseUrl is null or empty.</exception>
        public CursorUrlResultTranformer(string baseUrl)
        {
            Guard.NotNull(baseUrl, nameof(baseUrl));
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Transforms the specified cursor-based pagination result into a <see cref="CursorUrlPaginationResult{T, TKey}"/>,
        /// appending navigation URLs based on the cursor values.
        /// </summary>
        /// <param name="paginationResult">The pagination result to transform.</param>
        /// <param name="parameters">The pagination parameters used to generate the original result, 
        /// including the cursor and size information.</param>
        /// <returns>A <see cref="CursorUrlPaginationResult{T, TKey}"/> that includes navigational URLs for cursor-based pagination.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either the paginationResult or parameters is null.</exception>
        public CursorUrlPaginationResult<T, TKey> Transform(IPaginationResult<T> paginationResult)
        {
            Guard.NotNull(paginationResult, nameof(paginationResult));

            var cursorResult = paginationResult as CursorPaginationResult<T, TKey>;
            if (cursorResult == null) throw new InvalidOperationException("The pagination result is not a cursor pagination result.");

            var nextCursor = cursorResult.NextCursor;

            return new CursorUrlPaginationResult<T, TKey>
            {
                Items = cursorResult.Items,
                PageSize = cursorResult.PageSize,
                CurrentCursor = cursorResult.CurrentCursor,
                NextCursor = nextCursor,
                NextPageUrl = nextCursor != null ? $"{_baseUrl}?cursor={nextCursor}&pageSize={cursorResult.PageSize}" : null,
                CurrentUrl = $"{_baseUrl}?cursor={cursorResult.CurrentCursor}&pageSize={cursorResult.PageSize}"
            };
        }
    }

}
