
using System.Linq.Expressions;
using System.Reflection;
using CorePagination.Common;
using CorePagination.Contracts;
using CorePagination.Support;
using Microsoft.EntityFrameworkCore;

namespace CorePagination.Paginators.CursorPaginator { 
    public class CursorPaginator<T, TKey> : IPagination<T, CursorPaginationParameters<TKey>, CursorPaginationResult<T, TKey>>
        where T : class
        where TKey : IComparable
    {
        private readonly Expression<Func<T, TKey>> _keySelector;

        public CursorPaginator(Expression<Func<T, TKey>> keySelector)
        {
            _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
        }

        public async Task<CursorPaginationResult<T, TKey>> PaginateAsync(IQueryable<T> query, CursorPaginationParameters<TKey> parameters)
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNull(parameters, nameof(parameters));

            IQueryable<T> orderedQuery = parameters.Order == PaginationOrder.Ascending
                ? query.OrderBy(_keySelector)
                : query.OrderByDescending(_keySelector);

            if (parameters.CurrentCursor != null)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Invoke(_keySelector, parameter);
                var cursorValue = Expression.Constant(parameters.CurrentCursor, typeof(TKey));
                var comparison = parameters.Order == PaginationOrder.Ascending
                    ? Expression.GreaterThan(property, cursorValue)
                    : Expression.LessThan(property, cursorValue);
                var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);

                orderedQuery = orderedQuery.Where(lambda);
            }

            var itemsWithPossibleNext = await orderedQuery.Take(parameters.PageSize + 1).ToListAsync();
            var hasMore = itemsWithPossibleNext.Count > parameters.PageSize;
            var items = itemsWithPossibleNext.Take(parameters.PageSize).ToList();

            TKey nextCursor = default(TKey);
            if (hasMore)
            {
                var propInfo = typeof(T).GetProperty(((_keySelector.Body as MemberExpression)?.Member as PropertyInfo)?.Name);
                var lastItem = items.LastOrDefault();
                if (lastItem != null && propInfo != null)
                {
                    nextCursor = (TKey)propInfo.GetValue(lastItem);
                }
            }

            return new CursorPaginationResult<T, TKey>
            {
                Items = items,
                PageSize = parameters.PageSize,
                CurrentCursor = parameters.CurrentCursor,
                NextCursor = nextCursor,
                HasMore = hasMore
            };
        }
    }
}