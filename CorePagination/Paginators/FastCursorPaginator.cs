using CorePagination.Common;
using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Paginators.Fast
{
    
    public class CursorPaginationParameters<TKey> : PagePaginatorParameters
    {
        public TKey CurrentCursor { get; set; }
        public PaginationOrder Order { get; set; } = PaginationOrder.Ascending;
    }

    public class CursorPaginationResult<T, TKey> : IPaginationResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageSize { get; set; }
        public TKey CurrentCursor { get; set; }
        public TKey NextCursor { get; set; }
        public bool HasMore { get; set; }
        public int Page { get; set; }
        public int? TotalItems { get; set; }
    }

    public interface IIdentifiable<T>
    {
        T Id { get; }
    }

    public class FastCursorPaginator<T, TKey> : IPagination<T, CursorPaginationParameters<TKey>, CursorPaginationResult<T, TKey>>
    where T : class, IIdentifiable<TKey>
    where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        public async Task<CursorPaginationResult<T, TKey>> PaginateAsync(IQueryable<T> query, CursorPaginationParameters<TKey> parameters)
        {
            IQueryable<T> orderedQuery;
            if (parameters.Order == PaginationOrder.Ascending)
            {
                orderedQuery = parameters.CurrentCursor != null
                    ? query.Where(x => x.Id.CompareTo(parameters.CurrentCursor) > 0).OrderBy(x => x.Id)
                    : query.OrderBy(x => x.Id);
            }
            else
            {
                orderedQuery = parameters.CurrentCursor != null
                    ? query.Where(x => x.Id.CompareTo(parameters.CurrentCursor) < 0).OrderByDescending(x => x.Id)
                    : query.OrderByDescending(x => x.Id);
            }

            var items = await orderedQuery.Take(parameters.PageSize + 1).ToListAsync();
            var hasNextPage = items.Count > parameters.PageSize;
            var resultItems = hasNextPage ? items.Take(parameters.PageSize).ToList() : items;

            TKey nextCursor = hasNextPage ? resultItems.Last().Id : default;

            return new CursorPaginationResult<T, TKey>
            {
                Items = resultItems,
                PageSize = parameters.PageSize,
                CurrentCursor = parameters.CurrentCursor,
                NextCursor = nextCursor,
                HasMore = hasNextPage
            };
        }
    }

}
