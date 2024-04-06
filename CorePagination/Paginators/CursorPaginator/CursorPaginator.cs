
using System.Linq.Expressions;
using System.Reflection;
using CorePagination.Common;
using CorePagination.Contracts;
using CorePagination.Support;
using Microsoft.EntityFrameworkCore;

namespace CorePagination.Paginators.CursorPaginator {

    /// <summary>
    /// Represents a paginator that provides cursor-based pagination functionality.
    /// </summary>
    /// <typeparam name="T">The type of the elements to be paginated.</typeparam>
    /// <typeparam name="TKey">The type of the cursor property.</typeparam>
    public class CursorPaginator<T, TKey> : IPaginationAsync<T, CursorPaginationParameters<TKey>, CursorPaginationResult<T, TKey>>
        where T : class
        where TKey : IComparable
    {
        private readonly Expression<Func<T, TKey>> _keySelector;

        /// <summary>
        /// Initializes a new instance of the CursorPaginator class with the specified key selector.
        /// </summary>
        /// <param name="keySelector">The expression used to select the cursor property.</param>
        public CursorPaginator(Expression<Func<T, TKey>> keySelector)
        {
            Guard.NotNull(keySelector, nameof(keySelector));
            _keySelector = keySelector;
        }

        /// <summary>
        /// Paginates the provided IQueryable based on the given CursorPaginationParameters.
        /// </summary>
        /// <param name="query">The IQueryable to be paginated.</param>
        /// <param name="parameters">The parameters for cursor-based pagination, including page size, current cursor, and pagination order.</param>
        /// <returns>A CursorPaginationResult containing the paginated items, current cursor, next cursor, and a flag indicating if there are more items.</returns>
        public async Task<CursorPaginationResult<T, TKey>> PaginateAsync(IQueryable<T> query, CursorPaginationParameters<TKey> parameters)
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNull(parameters, nameof(parameters));
            Guard.GreaterThanZero(parameters.PageSize, nameof(parameters.PageSize));

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

        /// <summary>
        /// Paginates the provided IQueryable based on the given CursorPaginationParameters.
        /// </summary>
        /// <param name="query">The IQueryable to be paginated.</param>
        /// <param name="parameters">The parameters for cursor-based pagination, including page size, current cursor, and pagination order.</param>
        /// <returns>A CursorPaginationResult containing the paginated items, current cursor, next cursor, and a flag indicating if there are more items.</returns>
        public CursorPaginationResult<T, TKey> Paginate(IQueryable<T> query, CursorPaginationParameters<TKey> parameters)
        {
            Guard.NotNull(query, nameof(query));
            Guard.NotNull(parameters, nameof(parameters));
            Guard.GreaterThanZero(parameters.PageSize, nameof(parameters.PageSize));

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

            var itemsWithPossibleNext = orderedQuery.Take(parameters.PageSize + 1).ToList();
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