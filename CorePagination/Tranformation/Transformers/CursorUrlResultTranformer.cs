using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Paginators.Fast;
using CorePagination.Support;
using CorePagination.Tranformation.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Tranformation.Transformers
{
    public class CursorUrlResultTranformer<T, TKey> : IPaginationTranformer<T, CursorUrlPaginationResult<T, TKey>>
        where T : class
        where TKey : IComparable
    {
        private readonly string _baseUrl;

        public CursorUrlResultTranformer(string baseUrl)
        {
            Guard.NotNull(baseUrl, nameof(baseUrl));
            _baseUrl = baseUrl;
        }

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
